using System.Collections;
using System.Collections.Generic;
using ReactUnity;
using ReactUnity.Reactive;
using UnityEngine;

public class ReactUnityBridge : MonoBehaviour {
    [NonNullField]
    public UIRouter Router;

    public ReactiveValue<string> route = new();
    public ReactiveValue<string> debugGameState = new();
    
    // Game System References
    [NonNullField]
    public GameLifecycleManager GameLifecycleManager;

    void Awake() {
        ReactRendererBase reactRenderer = GetComponentInChildren<ReactUnity.UGUI.ReactRendererUGUI>();
        Router.OnRouteUpdate += OnRouteUpdate;
        reactRenderer.Globals["route"] = route;

        // Game System References
        reactRenderer.Globals["debugGameState"] = debugGameState;
        reactRenderer.Globals["gameLifecycleManager"] = GameLifecycleManager;
        GameLifecycleManager.OnGameStateUpdated += OnGameStateUpdated;
    }

    void OnRouteUpdate(object sender, string data) {
        route.Value = data;
    }

    void OnGameStateUpdated(object sender, GameLifecycleManager.GameState data)
    {
        debugGameState.Value = data.ToString();
    }
}
