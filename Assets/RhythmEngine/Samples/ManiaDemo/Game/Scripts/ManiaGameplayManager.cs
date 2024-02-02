using System;
using UnityEngine;

namespace RhythmEngine.Examples
{
    /// <summary>
    /// This class is responsible for managing the gameplay of the Mania demo.
    /// It gathers the inputs from the input manager and misses from the note manager and handles the judgements.
    /// Judgements for anyone who doesn't know are the "Perfect", "Good", "Miss" etc. that you see in rhythm games.
    /// </summary>
    public class ManiaGameplayManager : MonoBehaviour
    {
        [SerializeField] private RhythmEngineCore RhythmEngine;
        [SerializeField] private ManiaInputManager InputManager;
        [SerializeField] private ManiaNoteManager NoteManager;

        public enum Judgement { Miss, Good, Perfect }

        private SimpleManiaSong Song => RhythmEngine.Song as SimpleManiaSong; // We need to cast the song to our custom type to access the judgement timings

        public event Action<Judgement> OnJudgement; // Used by the UI to display the judgements

        private void OnEnable()
        {
            InputManager.OnKeyPressed += OnInput;
            NoteManager.OnMiss += MissJudgement;
        }

        private void OnDisable()
        {
            InputManager.OnKeyPressed -= OnInput;
            NoteManager.OnMiss -= MissJudgement;
        }

        private void OnInput(int key)
        {
            var currentTime = RhythmEngine.GetCurrentAudioTime();
            var note = NoteManager.GetClosestNoteToInput(key, currentTime);

            if (note == null) return;

            // If there's a note on a lane we clicked, we need to calculate the offset between the note's time and the current time
            var noteTime = note.Value.Time;
            var offset = Math.Abs(noteTime - currentTime);
            var offsetInMs = offset * 1000; // I prefer to work in milliseconds when it comes to judgements

            // We check the signed offset to see if we're too early to check
            if (offsetInMs > Song.GoodTimeMs)
            {
                // In this demo we don't apply a miss for that
                // Some games have a specific "miss" judgement for that, where they check if the offset was between the miss and worst judgement range.
                // Here we just skip the input
                return;
            }

            var absOffsetInMs = Math.Abs(offsetInMs);

            // We check the unsigned (absolute) offset to see if we're in the perfect or good range
            if (absOffsetInMs <= Song.PerfectTimeMs)
            {
                InputJudgement(Judgement.Perfect);
                NoteManager.DespawnNote(note.Value);
            }
            else if (absOffsetInMs <= Song.GoodTimeMs)
            {
                InputJudgement(Judgement.Good);
                NoteManager.DespawnNote(note.Value);
            }

            // "Too late" Miss should be caught by the ManiaNoteManager
        }

        private void MissJudgement() => InputJudgement(Judgement.Miss);

        private void InputJudgement(Judgement judgement)
        {
            OnJudgement?.Invoke(judgement);
        }
    }
}
