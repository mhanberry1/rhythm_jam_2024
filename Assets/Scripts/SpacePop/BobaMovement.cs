using System;
using UnityEngine;

namespace RhythmJam
{

public class BobaMovement : MonoBehaviour
{
    [SerializeField] private double Modifier = 0.2;
    [SerializeField] private Vector3 DivergencePosition;
    [SerializeField] private float DivergenceTransitionTime;

    private double _t = 0;
    private Vector3 _originalPos;

    private bool _divergenceTriggered;
    private float _divergenceProgress;

    void OnEnable() {
        CallResponseGameplayManager.Instance.OnSongEvent += OnSongEvent;
    }

    void OnDisable() {
        CallResponseGameplayManager.Instance.OnSongEvent -= OnSongEvent;
    }

    void Start()
    {
        _originalPos = transform.position;
    }

    void Update()
    {
        _t = _t < 2 * Math.PI ? _t + Time.deltaTime : 0;

        var divergencePosOffset = new Vector3();
        if (_divergenceTriggered) {
            if (_divergenceProgress < 1) {
                _divergenceProgress += (Time.deltaTime / DivergenceTransitionTime);
            }
            divergencePosOffset = DivergencePosition * _divergenceProgress;
        }

        transform.position = new Vector3(
            (float) (Modifier * Math.Cos(_t) + _originalPos.x),
            (float) (Modifier * Math.Sin(_t) + _originalPos.y),
            0
        ) + divergencePosOffset;
    }

    void OnSongEvent(object sender, SongEvent.EventType eventType) {
        if (eventType == SongEvent.EventType.Divergence) {
            _divergenceTriggered = true;
        }
    }
}

}
