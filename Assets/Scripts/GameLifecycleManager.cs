using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLifecycleManager : Singleton<GameLifecycleManager>
{
    public enum GameState
    {
        MainMenu,
        Instructions,
        GameStarted,
        GamePaused,
        GameOver,
    }

    public enum GameType
    {
        SpacePop,
        OldManRave,
        CutieGerms,
    }
    
    public event EventHandler<GameState> OnGameStateUpdated;

    private GameState _currentGameState = GameState.MainMenu;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
    }

    private GameType _currentGameType = GameType.SpacePop;
    
    public GameType CurrentGameType
    {
        get { return _currentGameType; }
    }

    private void SwitchGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MainMenu:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.MainMenu);
                break;
            case GameState.Instructions:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.Instructions);
                break;
            case GameState.GameStarted:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.Hud);
                // Unpause the game
                Time.timeScale = 1;
                break;
            case GameState.GamePaused:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.PauseMenu);
                // Pause the game
                Time.timeScale = 0;
                // PlayerManager.Instance.SwitchActionMaps("menu");
                break;
            case GameState.GameOver:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.MainMenu);
                break;
        }

        _currentGameState = gameState;
        OnGameStateUpdated?.Invoke(this, _currentGameState);
    }

    [JsCallable]
    public void StartGame(GameType gameType)
    {
        _currentGameType = gameType;
        SwitchGameState(GameState.GameStarted);
    }
    
    [JsCallable]
    public void PauseGame()
    {
        SwitchGameState(GameState.GamePaused);
    }
    
    [JsCallable]
    public void UnpauseGame()
    {
        SwitchGameState(GameState.GameStarted);
    }
    
    [JsCallable]
    public void ReturnToMainMenu()
    {
        SwitchGameState(GameState.MainMenu);
    }
    
    void Start()
    {
        SwitchGameState(_currentGameState);
    }
}
