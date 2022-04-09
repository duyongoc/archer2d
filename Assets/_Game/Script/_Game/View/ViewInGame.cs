using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ViewInGame : View
{


    // inspector
    [SerializeField] private TMP_Text textScore;
    [SerializeField] private GameObject prefabScore;
    [SerializeField] private Transform pointScore;
    [SerializeField] private List<Transform> arrowBunch;



    [Space(10)]
    [SerializeField] private Transform effectLastShoot;
    [SerializeField] private float timerLastShootAppear = 0.5f;
    [SerializeField] private float timerLastShootStay = 1.25f;

    [Space(10)]
    [SerializeField] private Transform effectPerfectShoot;
    [SerializeField] private float timerPerfectShootAppear = 0.5f;
    [SerializeField] private float timerPerfectShootStay = 1.25f;





    #region  UNITY
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    // private void Update()
    // {
    // }
    #endregion



    #region STATE
    public override void StartState()
    {
        base.StartState();
        StartView();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        UpdateView();
    }

    public override void EndState()
    {
        base.EndState();
        EndView();
    }
    #endregion


    private void StartView()
    {
        GameMgr.Instance.PlayGame();
    }

    private void UpdateView()
    {
    }

    private void EndView()
    {
    }


    public void UpdateArrowInGame(int arrow)
    {
        switch (arrow)
        {
            case 3:
                arrowBunch.ForEach(x => x.gameObject.SetActive(true));
                break;

            case 2:
                arrowBunch.ForEach(x => x.gameObject.SetActive(true));
                arrowBunch[2].gameObject.SetActive(false);
                break;

            case 1:
                arrowBunch.ForEach(x => x.gameObject.SetActive(true));
                arrowBunch[1].gameObject.SetActive(false);
                arrowBunch[2].gameObject.SetActive(false);
                break;

            case 0:
                arrowBunch.ForEach(x => x.gameObject.SetActive(false));
                break;
        }
    }


    public void UpdateScore(int score)
    {
        PlayScoreAnimation();
        textScore.text = score.ToString();
    }


    private void PlayScoreAnimation()
    {
        textScore.transform.DOScale(Vector3.one * 1.25f, 1).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                textScore.transform.localScale = Vector3.one;
            });
    }


    public void PlayerLastShootEffect()
    {
        effectLastShoot.gameObject.SetActive(true);
        effectLastShoot.localScale = Vector3.zero;

        Sequence effect = DOTween.Sequence();

        effect.Append(effectLastShoot.DOScale(Vector3.one * .5f, timerLastShootAppear))
            .AppendInterval(timerLastShootStay)
            .Append(effectLastShoot.DOScale(Vector3.zero, timerLastShootAppear))
            .OnComplete(() =>
            {
                effectLastShoot.gameObject.SetActive(true);
                effectLastShoot.localScale = Vector3.zero;
            });
    }

    public void PlayerPerfectShootEffect()
    {
        effectPerfectShoot.gameObject.SetActive(true);
        effectPerfectShoot.localScale = Vector3.zero;

        Sequence effect = DOTween.Sequence();

        effect.Append(effectPerfectShoot.DOScale(Vector3.one * .5f, timerPerfectShootAppear))
            .AppendInterval(timerPerfectShootAppear)
            .Append(effectPerfectShoot.DOScale(Vector3.zero, timerPerfectShootAppear))
            .OnComplete(() =>
            {
                effectPerfectShoot.gameObject.SetActive(true);
                effectPerfectShoot.localScale = Vector3.zero;
            });
    }


    public void CreateScoreUI(int score)
    {
        var scoreUI = prefabScore.SpawnToGarbage(pointScore.position, Quaternion.identity);
        scoreUI.GetComponent<TextScore>().Init(score);
    }


    public void ResetData()
    {
        textScore.text = "00";
    }


}
