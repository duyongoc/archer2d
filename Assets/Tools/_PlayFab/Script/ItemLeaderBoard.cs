using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLeaderBoard : MonoBehaviour
{

    public Text txtName;
    public Text txtScore;


    #region UNITY
    // private void Start()
    // {
    // }

    // private void Update()
    // {
    // }
    #endregion


    public void SetItem(string name, string score)
    {
        txtName.text = name;
        txtScore.text = score;

        if (string.IsNullOrEmpty(txtName.text))
        {
            txtName.text = "No name";
        }
    }


}
