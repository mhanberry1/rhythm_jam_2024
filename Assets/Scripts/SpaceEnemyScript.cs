using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmJam
{

public class SpaceEnemyScript : MonoBehaviour
{
    public CallResponseGameplayManager GameManager;

    private Vector3 _originalScale;
    private float _currentScale = 1.0f;

    void OnEnable() {
        GameManager.OnCallNote += OnCallNote;
    }

    void OnDisable() {
        GameManager.OnCallNote -= OnCallNote;
    }

    // Start is called before the first frame update
    void Start()
    {
        _originalScale = this.gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentScale > 1) {
            _currentScale -= 0.1f;
            this.gameObject.transform.localScale = _originalScale * _currentScale;
        }
    }

    void OnCallNote() {
        _currentScale = 1.5f;
    }
}

}
