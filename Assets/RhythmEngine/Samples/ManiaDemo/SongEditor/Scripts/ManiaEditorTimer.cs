using System;
using UnityEngine;
using UnityEngine.UI;

namespace RhythmEngine.Examples
{
    /// <summary>
    /// Simples class to display the current time of the song in the editor.
    /// </summary>
    public class ManiaEditorTimer : MonoBehaviour
    {
        [SerializeField] private RhythmEngineCore RhythmEngine;

        private Text _text;

        private void Start()
        {
            _text = GetComponent<Text>();
        }

        private void Update()
        {
            // We want to use the source start time when the song is paused, because the current audio time will not update when paused.
            _text.text = $"Current Time:\n{ToTime(RhythmEngine.IsPaused ? RhythmEngine.SourceStartTime : RhythmEngine.GetCurrentAudioTime())}";
        }

        private static string ToTime(double value) => $"{Math.Floor(value / 60):00}:{Math.Floor(value) % 60:00}.{Math.Floor(value * 1000 % 1000):000}";
    }
}
