using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TurnData
{
    public TypeTarget type = TypeTarget.None;

    [Space(10)]
    public float timeMove;
    public float highMin;
    public float highMax;
    public bool randomEase = false;

    // [Space(10)]
    // public float distanceMin;
    // public float distanceMax;
}


[CreateAssetMenu(fileName = "Setting", menuName = "Setting/Setting", order = 1)]
public class SettingSO : ScriptableObject
{
    public int arrows = 3;

    [Space(10)]
    public List<TurnData> turnEasy;
    public List<TurnData> turnMedium;
    public List<TurnData> turnHard;
    public List<TurnData> nightmare;
}
