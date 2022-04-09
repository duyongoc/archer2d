using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{

    [Space(10)]
    public List<Fruit> fruitList;


    #region UNITY
    private void Start()
    {
        CacheComponent();
        Init();
    }

    // private void Update()
    // {
    // }
    #endregion



    private void Init()
    {
        fruitList.ForEach(x => x.Init());
    }


    public void RepareForNextTurn()
    {
        fruitList.ForEach(x => x.Hide());
        PickRandomFruit();
    }


    private void PickRandomFruit()
    {
        var rand = Random.Range(0, fruitList.Count);
        fruitList[rand].Show();
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
    }


    private void CacheComponent()
    {
        var arrayFruit = GetComponentsInChildren<Fruit>();
        fruitList = new List<Fruit>(arrayFruit);
    }


    public void ResetData()
    {
        fruitList.ForEach(x => x.ResetData());
    }

}
