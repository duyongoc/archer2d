using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Background : MonoBehaviour
{

    // private
    [Space(10)]
    [SerializeField] private Transform _bgTransition;
    [SerializeField] private Vector3 _targetBgTransition;
    [SerializeField] private Vector3 _originBgTransition;


    #region UNITY
    private void Start()
    {
        Init();
    }

    // private void Update()
    // {
    // }
    #endregion


    private void Init()
    {
        _originBgTransition = _bgTransition.position;
        HideBackgroundOnWeb();
    }


    public void TransitionBackground(float timeMove, Action callback)
    {
        ShowBackgroundOnWeb();
        SoundMgr.Instance.PlaySFX(SoundMgr.SFX_BG_TRANSITION);
        Sequence sqTransition = DOTween.Sequence();

        sqTransition.Append(_bgTransition.DOMove(new Vector3(0, 0, _bgTransition.position.z), timeMove)) //.SetEase(Ease.InBounce)
                    .AppendCallback(() => { callback?.Invoke(); }) // reset turn game here
                    .PrependInterval(1.25f) // delay with time
                    .Append(_bgTransition.DOMove(_targetBgTransition, timeMove)) //.SetEase(Ease.InBounce))
                    .OnComplete(() =>
                    {
                        ResetBackground();
                    });
    }


    private void ResetBackground()
    {
        HideBackgroundOnWeb();
        _bgTransition.position = _originBgTransition;
    }


    private void ShowBackgroundOnWeb()
    {
#if UNITY_WEBGL
        _bgTransition.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1);
#endif
    }


    private void HideBackgroundOnWeb()
    {
#if UNITY_WEBGL
        _bgTransition.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
#endif
    }


    public void ResetData()
    {
        ResetBackground();
    }


}
