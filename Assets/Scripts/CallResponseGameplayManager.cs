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
    public class CallResponseGameplayManager : Singleton<CallResponseGameplayManager>
    {
		[SerializeField] private InputActionAsset inputActions;
        [SerializeField] private RhythmEngineCore RhythmEngine;

        [NonNullField]
        public CallResponseSong SpacePopSong;

        [NonNullField]
        public CallResponseSong OldManRaveSong;
        
        [NonNullField]
        public CallResponseSong CutieGermsSong;
        
        public enum Judgement
        {
	        Good,
	        Perfect,
	        Miss
        }

		public event Action OnCallNote;
		public event Action OnMiss;
		public event Action OnGood;
		public event Action OnPerfect;
		
		public event EventHandler<Judgement> OnResponseNote;

        private CallResponseSong _currentSong;

		private Queue<CallResponseNote> _callNotes;
		private Queue<CallResponseNote> _responseNotes;

		private bool _isPlaying = false;

		protected override void Awake()
		{
			base.Awake();
			inputActions.FindActionMap("gameplay").Enable();
			inputActions.FindActionMap("gameplay").FindAction("beatInput").performed += OnBeatInput;
			
			// RhythmEngineCore wants this to be done before Start for some reason.
			RhythmEngine.SetSong(SpacePopSong);
			RhythmEngine.InitTime();
		}

		public void Initialize(GameLifecycleManager.GameType gameType)
		{
			switch (gameType)
			{
				case GameLifecycleManager.GameType.SpacePop:
					_currentSong = SpacePopSong;
					break;
				case GameLifecycleManager.GameType.OldManRave:
					_currentSong = OldManRaveSong;
					break;
				case GameLifecycleManager.GameType.CutieGerms:
					_currentSong = CutieGermsSong;
					break;
			}
			
			if (_currentSong == null)
			{
				Debug.LogError("[CallResponseGameplayManager] Current song is null!");
				return;
			}
			
			_callNotes = new Queue<CallResponseNote>(_currentSong.CallNotes.OrderBy(note => note.Time));
			_responseNotes = new Queue<CallResponseNote>(_currentSong.ResponseNotes.OrderBy(note => note.Time));
			
			RhythmEngine.SetSong(_currentSong);
		}

		public void Play()
		{
			RhythmEngine.InitTime();
			RhythmEngine.Play();
			_isPlaying = true;
		}
		
		private void Update()
		{
			if (!_isPlaying)
			{
				return;
			}
			
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
				Debug.Log("call");
			}
		}

		// Remove response notes that have passed and invoke event handler
		private void HandleResponseNotes(double time)
		{
			while(_responseNotes.Peek().Time + _currentSong.GoodTimeMs / 1000 < time)
			{
				_responseNotes.Dequeue();
				OnMiss?.Invoke();
				OnResponseNote?.Invoke(this, Judgement.Miss);
				Debug.Log("miss");
			}
		}

		// Check if the user hit the notes on-time
		private void OnBeatInput(InputAction.CallbackContext context)
		{
			double accuracy = Math.Abs(RhythmEngine.GetCurrentAudioTime() - _responseNotes.Peek().Time) * 1000;

			if(accuracy < _currentSong.PerfectTimeMs)
			{
				_responseNotes.Dequeue();
				OnPerfect?.Invoke();
				OnResponseNote?.Invoke(this, Judgement.Perfect);
				Debug.Log("perfect");
			} else if (accuracy < _currentSong.GoodTimeMs)
			{
				_responseNotes.Dequeue();
				OnGood?.Invoke();
				OnResponseNote?.Invoke(this, Judgement.Good);
				Debug.Log("good");
			} else
			{
				OnMiss?.Invoke();
				OnResponseNote?.Invoke(this, Judgement.Miss);
				Debug.Log("miss");
			}
		}
    }
}
