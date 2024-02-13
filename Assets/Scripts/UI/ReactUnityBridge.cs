using System.Collections;
using System.Collections.Generic;
using ReactUnity;
using ReactUnity.Reactive;
using UnityEngine;

public class ReactUnityBridge : MonoBehaviour {
    [NonNullField]
    public UIRouter Router;

    public ReactiveValue<string> route = new ReactiveValue<string>();

    void Awake() {
        ReactRendererBase reactRenderer = GetComponentInChildren<ReactUnity.UGUI.ReactRendererUGUI>();
        Router.OnRouteUpdate += OnRouteUpdate;
        reactRenderer.Globals["route"] = route;
    }

    void OnRouteUpdate(object sender, string data) {
        route.Value = data;
    }
}
