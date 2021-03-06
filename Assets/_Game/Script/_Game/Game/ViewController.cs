using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ViewController : MonoBehaviour
{


    // [Inject] GameMgr _gameMgr;

    // private
    private View _stateView;

    // DI
    [Inject] private ViewLoading _viewLoading;
    [Inject] private ViewMenu _viewMenu;
    [Inject] private ViewInGame _viewInGame;
    [Inject] private ViewGameOver _viewGameOver;
    [Inject] private ViewSetting _viewSetting;
    [Inject] private ViewNone _viewNone;



    #region UNITY
    private void Start()
    {
        // Init();
    }

    private void Update()
    {
        _stateView.UpdateState();
    }
    #endregion



    private void Init()
    {
        SetState(_viewNone);
    }


    public void SetStateView(string viewName)
    {
        switch(viewName)
        {
            case "Loading": SetState(_viewLoading); break;
            case "Menu": SetState(_viewMenu); break;
            case "InGame": SetState(_viewInGame); break;
            case "GameOver": SetState(_viewGameOver); break;
            case "Setting": SetState(_viewSetting); break;
        }
    }


    public void SetState(View newState)
    {
        if (_stateView != null)
            _stateView.EndState();

        _stateView = newState;
        _stateView.StartState();

        //active view with current state
        SetActiveView(_stateView.GetType().Name);
    }


    public void SetActiveView(string nameScene)
    {
        _viewLoading.gameObject.SetActive(_viewLoading.name.Contains(nameScene));
        _viewMenu.gameObject.SetActive(_viewMenu.name.Contains(nameScene));
        _viewInGame.gameObject.SetActive(_viewInGame.name.Contains(nameScene));
        _viewGameOver.gameObject.SetActive(_viewGameOver.name.Contains(nameScene));
        _viewSetting.gameObject.SetActive(_viewSetting.name.Contains(nameScene));
        _viewNone.gameObject.SetActive(_viewNone.name.Contains(nameScene));
    }


}
