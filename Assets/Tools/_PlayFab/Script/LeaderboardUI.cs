using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{


    [Header("Leader Board")]
    public Transform contentLeaderBoard;
    public ItemLeaderBoard itemLeaderBoard;

    [Header("Pop up input name")]
    public GameObject uiPopup;
    public InputField inputName;
    public Text txtYourScore;
    public Text txtInfomation;


    #region UNITY
    private void Start()
    {
        // Init();
    }

    // private void Update()
    // {
    // }
    #endregion


    private void Init()
    {
        ShowPopup(false);
        ShowYourScore();
        txtInfomation.gameObject.SetActive(false);
    }


    private void ShowYourScore()
    {
        PlayfabManager.Instance.GetDisplayName((name) =>
        {
            print("boom " + name);
            txtYourScore.text = $"Your name - {name}: {ScoreMgr.Instance.CurrentScore}";
        });
    }


    public void ShowLeaderBoardUI()
    {
        Init();
        RefeshLeaderBoard();

        var request = new GetLeaderboardRequest
        {
            StatisticName = "jump2d",
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(request, (result) =>
        {
            // get leader board and show it
            result.Leaderboard.ForEach(player =>
            {
                var item = Instantiate(itemLeaderBoard, contentLeaderBoard);
                item.SetItem(player.DisplayName, player.StatValue.ToString());
            });
        }, (error) => { });
    }


    public void OnClickButtonPostScore()
    {
        ShowPopup(true);
    }


    public void OnClickButtonPost()
    {
        if (string.IsNullOrEmpty(inputName.text))
        {
            ShowTextWrongName();
            return;
        }

        ShowTextLoading();
        var currentScore = ScoreMgr.Instance.CurrentScore;

        PlayfabManager.Instance.SetDisplayName(inputName.text);
        PlayfabManager.Instance.SendLeaderboard(currentScore, () =>
        {
            ShowTextPostSucces();
            ShowLeaderBoardUI();
        });
    }


    private void RefeshLeaderBoard()
    {
        foreach (Transform child in contentLeaderBoard.transform)
        {
            if (child != null)
                Destroy(child.gameObject);
        }

    }

    private void ShowPopup(bool value)
    {
        uiPopup.SetActive(value);
    }

    private void ShowTextWrongName()
    {
        txtInfomation.gameObject.SetActive(true);
        txtInfomation.text = "Please enter your name!";
        txtInfomation.color = Color.red;
    }

    private void ShowTextPostSucces()
    {
        txtInfomation.gameObject.SetActive(true);
        txtInfomation.text = "Post score success!";
        txtInfomation.color = Color.green;
    }

    private void ShowTextLoading()
    {
        txtInfomation.gameObject.SetActive(true);
        txtInfomation.text = "Loading...";
        txtInfomation.color = Color.green;
    }



}
