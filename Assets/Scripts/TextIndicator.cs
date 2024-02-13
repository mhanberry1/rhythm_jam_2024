using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RhythmJam
{


public class TextIndicator : MonoBehaviour
{
    public CallResponseGameplayManager GameManager;

    private float _currentAlpha = 0.0f;
    private TMP_Text _text;

    void OnEnable() {
        GameManager.OnPerfect += OnPerfect;
        GameManager.OnGood += OnGood;
        GameManager.OnMiss += OnMiss;
    }

    void OnDisable() {
        GameManager.OnPerfect -= OnPerfect;
        GameManager.OnGood -= OnGood;
        GameManager.OnMiss -= OnMiss;
    }

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentAlpha > 0) {
            _currentAlpha -= 0.01f;
        }
        Color color = _text.color;
        color.a = _currentAlpha;
        _text.color = color;
    }

    void OnPerfect() {
        _text.text = "Perfect!";
        _currentAlpha = 1f;
    }

    void OnGood() {
        _text.text = "Good";
        _currentAlpha = 1f;
    }

    void OnMiss() {
        _text.text = "Miss :(";
        _currentAlpha = 1f;
    }

}

}
