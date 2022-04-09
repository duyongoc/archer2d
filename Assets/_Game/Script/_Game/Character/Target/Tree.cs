using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tree : MonoBehaviour
{

    [Header("Setting")]
    public Transform treeRoot;
    public Transform targetOnTree;
    public Transform stumpBody;

    [Space(10)]
    public List<GameObject> stumpDecorList;


    // private
    private TurnData _turn;
    private Vector3 _originBodyScale;



    #region UNITY
    private void Start()
    {
        _originBodyScale = stumpBody.localScale;

        // StandOnTree(turn.standMin, turn.standMax);
        // JumpingTree(turn.highMin, turn.highMax, turn.timeMove);
        // StandTreeRootOnX(currentTurn.distanceMin, currentTurn.distanceMax);
        // StandTreeOnY(currentTurn.highMin, currentTurn.highMax);
    }

    // private void Update()
    // {
    // }
    #endregion


    public void MoveTreePattern(TurnData newTurn)
    {
        _turn = newTurn;
        stumpBody.DOKill();
        targetOnTree.DOKill();

        switch (_turn.type)
        {
            // case TypeTarget.MoveXY:
            // StandTreeOnXY(); break;

            case TypeTarget.MoveY:
                StandTreeOnY(_turn.highMin, _turn.highMax); break;

            case TypeTarget.Jumping:
                JumpingTree(_turn.highMin, _turn.highMax, _turn.timeMove); break;

            case TypeTarget.None:
                StandNone();
                break;
        }
    }


    // public void StandTreeRootOnX(float distaneMin, float distanceMax)
    // {
    //     var rand = Random.Range(distaneMin, distanceMax);
    //     treeRoot.localPosition = new Vector3(rand, 0f, targetOnTree.localPosition.z);
    //     // treeRoot.localPosition = new Vector3(rand, targetOnTree.localPosition.y, targetOnTree.localPosition.z);
    // }

    // public void StandTreeOnXY()
    // {
    //     StandTreeRootOnX(_turn.distanceMin, _turn.distanceMax);
    //     StandTreeOnY(_turn.highMin, _turn.highMax);
    // }


    public void StandNone()
    {
        HideDecors();

        targetOnTree.localPosition = Vector3.zero;
        // stumpBody.localScale = _originBodyScale;
    }


    public void StandTreeOnY(float highMin, float highMax)
    {
        ShowDecors();

        var rand = Random.Range(highMin, highMax);
        targetOnTree.localPosition = new Vector3(targetOnTree.localPosition.x, rand, targetOnTree.localPosition.z);
        stumpBody.localScale = GetStumpBodyScale(rand);
    }


    public void JumpingTree(float highMin, float highMax, float time)
    {
        ShowDecors();

        var min = Random.Range(highMin, highMax / 2);
        var max = Random.Range(highMax / 2, highMax);

        var vecMin = new Vector3(targetOnTree.localPosition.x, min, targetOnTree.localPosition.z);
        var vecMax = new Vector3(targetOnTree.localPosition.x, max, targetOnTree.localPosition.z);

        // rand dom ease for nightmare mode
        var easeRand = Ease.InOutQuad;
        if(_turn.randomEase)
        {
            var rand = Random.Range(0, 36);
            easeRand = (Ease)rand;
        }

        var scaleMin = GetStumpBodyScale(min);
        var scaleMax = GetStumpBodyScale(max);

        // make anim for the target
        targetOnTree.localPosition = vecMin;
        targetOnTree.DOLocalMove(vecMax, time).SetLoops(-1, LoopType.Yoyo).SetEase(easeRand);

        // make anim for the tree
        stumpBody.localScale = GetStumpBodyScale(min);
        stumpBody.DOScale(scaleMax, time).SetLoops(-1, LoopType.Yoyo).SetEase(easeRand); ;
    }


    private Vector3 GetStumpBodyScale(float distanceMax)
    {
        var scale = stumpBody.localScale;

        // if distancemax < 1 => there is a space in tree
        if (distanceMax < 1) scale = new Vector3(scale.x, 0.35f + distanceMax / 2.5f, scale.z);
        else scale = new Vector3(scale.x, 0.35f + distanceMax / 1.7f, scale.z);

        return scale;
    }


    private void ShowDecors()
    {
        stumpDecorList.ForEach(x => x.SetActive(true));
    }

    private void HideDecors()
    {
        stumpDecorList.ForEach(x => x.SetActive(false));
    }

    public void ResetData()
    {
        targetOnTree.DOKill();
    }



}
