using System;
using System.Collections;
using System.Collections.Generic;
using RhythmJam;
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
    public event EventHandler<int> OnScoreUpdated;
    public event EventHandler<string> OnStatusUpdated;

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

    private int _score = 0;
    public int Score
    {
        get { return _score; }
    }

    private string _status = "GameOver";
    public string Status
    {
        get { return _status; }
    }

    private void SwitchGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MainMenu:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.MainMenu);
                LevelManager.Instance.DisableLevel();
                break;
            case GameState.Instructions:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.Instructions);
                break;
            case GameState.GameStarted:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.Hud);
                LevelManager.Instance.SwitchLevels(_currentGameType);
                CallResponseGameplayManager.Instance.Play();
                SetScore(0);
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
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.GameOver);
                break;
        }

        _currentGameState = gameState;
        OnGameStateUpdated?.Invoke(this, _currentGameState);
    }

    public void SetScore(int score)
    {
        _score = score;
        OnScoreUpdated?.Invoke(this, _score);
    }

    public void SetStatus(string status)
    {
        _status = status;
        OnStatusUpdated?.Invoke(this, _status);
    }

    [JsCallable]
    public void StartGame(GameType gameType)
    {
        _currentGameType = gameType;
        CallResponseGameplayManager.Instance.Initialize(_currentGameType);
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

    [JsCallable]
    public void EndGame()
    {
        SwitchGameState(GameState.GameOver);
    }

    void Start()
    {
        SwitchGameState(_currentGameState);
    }
}
