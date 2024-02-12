using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using RhythmEngine;

namespace RhythmJam
{
    /// <summary>
	/// Provides events to handle note interactions
    /// </summary>
    public class CallResponseNoteManager : MonoBehaviour
    {
		[SerializeField] private InputActionAsset inputActions;
        [SerializeField] private RhythmEngineCore RhythmEngine;

		public event Action OnCallNote;
		public event Action OnMiss;
		public event Action OnGood;
		public event Action OnPerfect;

        private CallResponseSong Song => RhythmEngine.Song as CallResponseSong;

		private Queue<CallResponseNote> _callNotes;
		private Queue<CallResponseNote> _responseNotes;

		private void Awake()
		{
			inputActions.FindActionMap("gameplay").Enable();
			inputActions.FindActionMap("gameplay").FindAction("beatInput").performed += OnBeatInput;

            _callNotes = new Queue<CallResponseNote>(Song.CallNotes.OrderBy(note => note.Time));
			_responseNotes = new Queue<CallResponseNote>(Song.ResponseNotes.OrderBy(note => note.Time));
		}

		private void Update()
		{
			double time = RhythmEngine.GetCurrentAudioTime();
			HandleCallNotes(time);
			HandleResponseNotes(time);
		}

		// Remove call notes that have passed and invoke event handler
		private void HandleCallNotes(double time)
		{
			while(_callNotes.Peek().Time <= time)
			{
				_callNotes.Dequeue();
				OnCallNote?.Invoke();
			}
		}

		// Remove response notes that have passed and invoke event handler
		private void HandleResponseNotes(double time)
		{
			while(_responseNotes.Peek().Time <= time)
			{
				_responseNotes.Dequeue();
				OnMiss?.Invoke();
			}
		}

		// Check if the user hit the notes on-time
		private void OnBeatInput(InputAction.CallbackContext context)
		{
			double accuracy = Math.Abs(RhythmEngine.GetCurrentAudioTime() - _responseNotes.Peek().Time) * 1000;
			Debug.Log(accuracy);

			if(accuracy < Song.PerfectTimeMs)
			{
				_responseNotes.Dequeue();
				OnPerfect?.Invoke();
			} else if (accuracy < Song.GoodTimeMs)
			{
				_responseNotes.Dequeue();
				OnGood?.Invoke();
			} else
			{
				OnMiss?.Invoke();
			}
		}
    }
}
