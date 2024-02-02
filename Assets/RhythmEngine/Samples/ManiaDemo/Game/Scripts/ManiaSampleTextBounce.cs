using System;
using UnityEngine;

namespace RhythmEngine.Examples
{
    /// <summary>
    /// This class bounces the sample text to the beat given by the BeatSequencer.
    /// </summary>
    public class ManiaSampleTextBounce : MonoBehaviour
    {
        [SerializeField] private BeatSequencer BeatSequencer;

        // We only want to watch for the kick and the snare in the sequence.
        [SerializeField] private int[] InstrumentsToWatch = { 0, 1 };

        [Space]
        [SerializeField] private Vector3 BeatScale = new Vector3(1.1f, 1.1f, 1f);
        [SerializeField] private float LerpSpeed = 3f;

        private Vector3 _defaultScale;

        private void Start()
        {
            _defaultScale = transform.localScale;
        }

        private void OnEnable()
        {
            // Here we use the simplest way of using the BeatSequencer, by subscribing to the OnInstrumentStep event.
            BeatSequencer.OnInstrumentStep += OnInstrumentStep;
        }

        private void OnDisable()
        {
            BeatSequencer.OnInstrumentStep -= OnInstrumentStep;
        }

        private void OnInstrumentStep(int instrument)
        {
            // If the instrument is not in the list of instruments we want to watch, we don't want to scale up.
            if (Array.IndexOf(InstrumentsToWatch, instrument) == -1) return;

            transform.localScale = BeatScale;
        }

        private void Update()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _defaultScale, Time.deltaTime * LerpSpeed);
        }
    }
}
