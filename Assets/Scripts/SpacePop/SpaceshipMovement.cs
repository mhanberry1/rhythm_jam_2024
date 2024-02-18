using System;
using UnityEngine;

namespace RhythmJam
{

public class SpaceshipMovement : MonoBehaviour
{
    [SerializeField] private double Modifier = 0.3;

    private double _t = 0;
    private Vector3 _originalPos;
    private Shaker _shaker;

    void OnEnable() {
        CallResponseGameplayManager.Instance.OnResponseNote += OnResponseNote;
    }

    void OnDisable() {
        CallResponseGameplayManager.Instance.OnResponseNote -= OnResponseNote;
    }

    void Start()
    {
        _originalPos = transform.position;
        _shaker = GetComponent<Shaker>();
    }

    void Update()
    {
        _t = _t < 2 * Math.PI ? _t + Time.deltaTime: 0;

        transform.position = new Vector3 (
            (float) (Modifier * Math.Cos(_t) + _originalPos.x),
            _originalPos.y,
            0
        );
    }

    void OnResponseNote(object sender, CallResponseGameplayManager.Judgement judgement) {
        switch (judgement) {
            case CallResponseGameplayManager.Judgement.Miss:
                // Shake
                _shaker.Begin();
                break;
            case CallResponseGameplayManager.Judgement.Good:
                // 
                break;
            case CallResponseGameplayManager.Judgement.Perfect:
                break;
        }
    }

}

}