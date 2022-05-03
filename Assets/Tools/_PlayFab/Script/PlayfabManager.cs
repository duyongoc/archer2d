using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayfabManager : Singleton<PlayfabManager>
{

    public string localPlayFabId = "NotSet";


    #region UNITY
    private void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            PlayFabSettings.staticSettings.TitleId = "9106F";
            // PlayFabSettings.staticSettings.DeveloperSecretKey = "MY4XI7NCDOCOG9A68P1C1RUXJE7EUW83HU69SIHOY535T4QJPY";
        }

        RequestLogin();
    }

    // private void Update()
    // {
    // }
    #endregion


    private void RequestLogin()
    {
        var request = new LoginWithCustomIDRequest { CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request,
            result =>
            {
                GetLeaderboard();
                GetAccountInfo();
            },
            error => Debug.Log(error.GenerateErrorReport()));
    }

    private void GetAccountInfo()
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request,
            result =>
            {
                localPlayFabId = result.AccountInfo.PlayFabId;
            },
            error => Debug.Log(error.GenerateErrorReport()));
    }

    public void GetPlayerProfile()
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = localPlayFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        },
        result => Debug.Log("The player's DisplayName profile data is: " + result.PlayerProfile),
        error => Debug.LogError(error.GenerateErrorReport()));
    }


    public void GetDisplayName(Action<string> cb_success = null)
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = localPlayFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        },
        result => cb_success?.Invoke(result.PlayerProfile.DisplayName),
        error => Debug.LogError(error.GenerateErrorReport()));
    }


    public void SetDisplayName(string name)
    {
        var requestName = new UpdateUserTitleDisplayNameRequest { DisplayName = name, };
        PlayFabClientAPI.UpdateUserTitleDisplayName(requestName,
            result => { },
            error => Debug.LogError(error.GenerateErrorReport()));
    }


    public void SendLeaderboard(int score, Action cb_success = null)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "jump2d",  Value = score  }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request,
            result => { cb_success?.Invoke(); },
            error => Debug.LogError(error.GenerateErrorReport()));
    }


    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "jump2d",
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(request,
            result =>
            {
                result.Leaderboard.ForEach(x => { Debug.Log($"x position{x.Position} {x.PlayFabId} {x.StatValue}"); });
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }




}
