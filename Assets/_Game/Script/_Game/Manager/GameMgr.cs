using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameMgr : Singleton<GameMgr>
{

    // public
    public static Action EVENT_RESET_INGAME;
    public GameState gameState = GameState.None;


    // DI
    [Inject] private ViewController _viewMgr;


    // properties
    public bool IsInGameState => gameState == GameState.InGame;



    #region UNITY
    private void Start()
    {
        InitGame();
    }

    // private void Update()
    // {
    // }
    #endregion



    private void InitGame()
    {
        SetState(gameState);
        SoundMgr.PlayMusic(SoundMgr.MUSIC_BACKGROUND);
    }


    public void PlayGame()
    {
        GameController.Instance.StartGame();
        SoundMgr.PlayMusic(SoundMgr.SFX_START_GAME);
    }


    public void ReplayGame()
    {
        GameController.Instance.ResetInGame();
        GameMgr.Instance.SetState(GameState.InGame);

        // prevent the user can click vo_toi_va
        GameController.Instance.SetCanShoot();
    }



    public void SetState(GameState newState)
    {
        gameState = newState;
        switch (gameState)
        {
            case GameState.Loading: _viewMgr.SetStateView("Loading"); break;
            case GameState.Menu: _viewMgr.SetStateView("Menu"); break;
            case GameState.InGame: _viewMgr.SetStateView("InGame"); break;
            case GameState.GameOver: _viewMgr.SetStateView("GameOver"); break;
            case GameState.Setting: _viewMgr.SetStateView("Setting"); break;
            case GameState.None: _viewMgr.SetStateView("None"); break;
        }
    }




}



// God bless my code to be bug free 
//
//                       _oo0oo_
//                      o8888888o
//                      88" . "88
//                      (| -_- |)
//                      0\  =  /0
//                    ___/`---'\___
//                  .' \\|     |// '.
//                 / \\|||  :  |||// \
//                / _||||| -:- |||||- \
//               |   | \\\  -  /// |   |
//               | \_|  ''\---/''  |_/ |
//               \  .-\__  '-'  ___/-. /
//             ___'. .'  /--.--\  `. .'___
//          ."" '<  `.___\_<|>_/___.' >' "".
//         | | :  `- \`.;`\ _ /`;.`/ - ` : | |
//         \  \ `_.   \_ __\ /__ _/   .-` /  /
//     =====`-.____`.___ \_____/___.-`___.-'=====
//                       `=---='
//
//
//     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//
//               佛祖保佑         永无BUG
//