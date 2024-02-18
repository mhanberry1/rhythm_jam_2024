using System.Collections;
using System.Collections.Generic;
using ReactUnity;
using ReactUnity.Reactive;
using UnityEngine;

public class ReactUnityBridge : MonoBehaviour {
    [NonNullField]
    public UIRouter Router;

    public ReactiveValue<string> route = new();
    public ReactiveValue<bool> debugModeEnabled = new();
    public ReactiveValue<string> debugGameState = new();
    public ReactiveValue<string> status = new();
    public ReactiveValue<int> score = new();
    public ReactiveValue<bool> canContinue = new();
    public ReactiveValue<Leaderboards.LeaderboardScores> leaderboardScores = new();
    
    // Game System References
    [NonNullField]
    public GameLifecycleManager GameLifecycleManager;
    [NonNullField]
    public Leaderboards Leaderboards;

    void Awake() {
        ReactRendererBase reactRenderer = GetComponentInChildren<ReactUnity.UGUI.ReactRendererUGUI>();
        Router.OnRouteUpdate += OnRouteUpdate;
        reactRenderer.Globals["route"] = route;
        reactRenderer.Globals["leaderboardScores"] = leaderboardScores;
        reactRenderer.Globals["score"] = score;
        reactRenderer.Globals["status"] = status;
        reactRenderer.Globals["debugGameState"] = debugGameState;
        reactRenderer.Globals["canContinue"] = canContinue;
        reactRenderer.Globals["debugModeEnabled"] = debugModeEnabled;
        
        // Game System References   
        reactRenderer.Globals["gameLifecycleManager"] = GameLifecycleManager;
        
        GameLifecycleManager.OnGameStateUpdated += OnGameStateUpdated;
        GameLifecycleManager.OnScoreUpdated += OnScoreUpdated;
        GameLifecycleManager.OnStatusUpdated += OnStatusUpdated;
        Leaderboards.OnLeaderboardScoresUpdated += LeaderboardsOnOnLeaderboardScoresUpdated;

        debugModeEnabled.Value = false;
#if UNITY_EDITOR
        debugModeEnabled.Value = true;
#endif
    }

    private void OnScoreUpdated(object sender, int data)
    {
        score.Value = data;
    }

    private void OnStatusUpdated(object sender, string data)
    {
        status.Value = data;
    }

    private void LeaderboardsOnOnLeaderboardScoresUpdated(object sender, Leaderboards.LeaderboardScores data)
    {
        leaderboardScores.Value = data;
    }

    void OnRouteUpdate(object sender, string data) {
        route.Value = data;
    }

    void OnGameStateUpdated(object sender, GameLifecycleManager.GameState data)
    {
        debugGameState.Value = data.ToString();
        canContinue.Value = GameLifecycleManager.CanContinue;
    }
}
