using System;
using UnityEngine;
using UnityEngine.UI;

namespace RhythmJam
{
    /// <summary>
    /// This text changes based on the call/response note.
    /// </summary>
    public class CallResponseText : MonoBehaviour
    {
        [SerializeField] private CallResponseAnnouncer _announcer;
        [SerializeField] private float _fadeSpeed = 0.05f;

        private Text _text;
        Color _originalColor;
        Color _currentColor;

        private void Start()
        {
            _text = GetComponent<Text>();
            _originalColor = _text.color;
            _currentColor = _originalColor;
        }

        private void OnEnable()
        {
            // Subscribe to events.
            _announcer.OnCallNotePlayed += OnCallNotePlayed;
            _announcer.OnResponseNotePlayed += OnResponseNotePlayed;
        }

        private void OnDisable()
        {
            _announcer.OnCallNotePlayed -= OnCallNotePlayed;
            _announcer.OnResponseNotePlayed -= OnResponseNotePlayed;
        }

        private void OnCallNotePlayed(CallResponseNote note)
        {
            _text.text = "Call";
            _currentColor = _originalColor;
        }

        private void OnResponseNotePlayed(CallResponseNote note)
        {
            _text.text = "Response";
            _currentColor = _originalColor;
        }

        private void Update()
        {
            _text.color = _currentColor;
            if (_currentColor.a > 0) {
                _currentColor.a -= _fadeSpeed;
            }
        }
    }
}
