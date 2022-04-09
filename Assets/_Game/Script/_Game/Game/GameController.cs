using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class GameController : Singleton<GameController>
{

    [Header("Config")]
    public LevelDesign levelDesign;
    public TurnData currentTurn;
    public int turnIndex = 0;
    public int arrows;

    [Header("Setting")]
    [SerializeField] private Background background;
    [SerializeField] private PlayerBow playerBow;
    [SerializeField] private PlayerTarget playerTarget;
    [SerializeField] private Tree tree;


    [Header("Turn Transition")]
    [SerializeField] private float _timeTransition;


    // DI
    [Inject] ViewInGame _viewInGame;


    private bool _canShot = false; // prevent shoot when click on the menu ui
    private bool _isTransition = false; // prevent shoot when loading transition background
    private bool _hitTheTarget = false;

    public bool CanShot => _canShot;
    public bool IsTransition => _isTransition;



    #region UNITY
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            NextTurn();
        }
    }
    #endregion



    public void Init()
    {
        arrows = levelDesign.GetArrows;
    }


    public void StartGame()
    {
        RefeshForNextTurn();
    }


    public void SetCanShoot()
    {
        _canShot = false;
        DG.Tweening.DOVirtual.DelayedCall(0.35f, () => { _canShot = true; });
    }


    public void AddScore(int score)
    {
        ScoreMgr.Instance.UpdateScore(score);
        _viewInGame.CreateScoreUI(score);
        _viewInGame.UpdateScore(ScoreMgr.Instance.score);
    }


    public void NextTurn()
    {
        turnIndex++;
        _isTransition = false;
        background.TransitionBackground(_timeTransition, () => { RefeshForNextTurn(); });
    }


    public void RefeshForNextTurn()
    {
        _isTransition = true;
        _hitTheTarget = false;
        arrows = levelDesign.GetArrows;
        _viewInGame.UpdateArrowInGame(arrows);

        currentTurn = levelDesign.GetTurn(turnIndex);
        tree.MoveTreePattern(currentTurn);

        playerBow.ResetTurn();
        playerTarget.RepareForNextTurn();
    }


    public void HitTheTarget(bool value)
    {
        _hitTheTarget = value;
    }


    public void ShakeCameraHitTheTaget()
    {
        var camera = Camera.main;
        camera.DOShakeRotation(0.25f, new Vector3(1, 1, 0), 20, 0)
            .OnComplete(() => { camera.transform.localRotation = Quaternion.identity; });
    }

    public void ShakeCameraGameOver()
    {
        var camera = Camera.main;
        camera.DOShakeRotation(0.5f, new Vector3(2, 2, 0), 20, 0)
            .OnComplete(() => { camera.transform.localRotation = Quaternion.identity; });
    }


    public void MissingArrow()
    {
        arrows--;
        _viewInGame.UpdateArrowInGame(arrows);

        // check the last arrow
        if (arrows == 1 && !_hitTheTarget)
        {
            PlayerLastShootEffect();
            return;
        }

        if (arrows <= 0)
        {
            NotifyEndAction();
        }
    }


    public void PlayerLastShootEffect()
    {
        _viewInGame.PlayerLastShootEffect();
    }

    public void PlayerPerfectShootEffect()
    {
        _viewInGame.PlayerPerfectShootEffect();
    }


    public void NotifyEndAction()
    {
        if (!_hitTheTarget)
        {
            GameMgr.Instance.SetState(GameState.GameOver);
            return;
        }

        // player pass the level
        NextTurn();
        playerBow.DecreaseSpaceShoot(turnIndex);
        SoundMgr.Instance.PlaySFX(SoundMgr.SFX_SHOOT);
    }


    public void ResetInGame()
    {
        // for game
        turnIndex = 0;
        tree.ResetData();
        playerBow.ResetData();
        playerTarget.ResetData();
        background.ResetData();

        // for ui
        _viewInGame.ResetData();

        GameMgr.EVENT_RESET_INGAME?.Invoke();
    }



}
