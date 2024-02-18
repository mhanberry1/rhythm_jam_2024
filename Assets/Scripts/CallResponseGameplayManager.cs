using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RhythmJam
{
    /// <summary>
    /// Provides events to handle note interactions
    /// </summary>
    public class CallResponseGameplayManager : Singleton<CallResponseGameplayManager>
    {
        [SerializeField] private InputActionAsset inputActions;
        public BoRhythmEngineCore RhythmEngine;

        [NonNullField]
        public CallResponseSong SpacePopSong;

        [NonNullField]
        public CallResponseSong OldManRaveSong;

        public enum Judgement
        {
            Good,
            Perfect,
            Miss,
            NoNote
        }

        public event Action OnCallNote;
        public event Action OnMiss;
        public event Action OnGood;
        public event Action OnPerfect;

        public event EventHandler<Judgement> OnResponseNote;
        public event EventHandler<SongEvent.EventType> OnSongEvent;
        // Called each beat, with beat number
        public event EventHandler<int> OnBeat;

        private CallResponseSong _currentSong;
        public CallResponseSong CurrentSong
        {
            get { return _currentSong; }
        }

        private Queue<CallResponseNote> _callNotes;
        private Queue<CallResponseNote> _responseNotes;
        private Queue<SongEvent> _songEvents;
        private int _nextBeat = 1;

        private bool _isPlaying = false;

        protected override void Awake()
        {
            base.Awake();
            inputActions.FindActionMap("gameplay").Enable();
            inputActions.FindActionMap("gameplay").FindAction("beatInput").performed += OnBeatInput;
        }

        public void Initialize(GameLifecycleManager.GameType gameType)
        {
            Debug.Log("GameType: " + gameType);
            switch (gameType)
            {
                case GameLifecycleManager.GameType.SpacePop:
                    _currentSong = SpacePopSong;
                    break;
                case GameLifecycleManager.GameType.OldManRave:
                    _currentSong = OldManRaveSong;
                    break;
            }

            if (_currentSong == null)
            {
                Debug.LogError("[CallResponseGameplayManager] Current song is null!");
                return;
            }

            // TODO: Handle other game types
            _callNotes = new Queue<CallResponseNote>(_currentSong.CallNotes.OrderBy(note => note.Time));
            _responseNotes = new Queue<CallResponseNote>(_currentSong.ResponseNotes.OrderBy(note => note.Time));
            _songEvents = new Queue<SongEvent>(_currentSong.SongEvents.OrderBy(e => e.Time));

            RhythmEngine.SetSong(_currentSong);
        }

        public void Play()
        {
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

            if (time >= _currentSong.Clip.length)
            {
                EndLevel();
            }

            HandleCallNotes(time);
            HandleResponseNotes(time);
            HandleSongEvent(time);
            HandleBeat(time);
        }

        // Remove call notes that have passed and invoke event handler
        private void HandleCallNotes(double time)
        {
            while(_callNotes.Count > 0 && _callNotes.Peek().Time <= time)
            {
                _callNotes.Dequeue();
                OnCallNote?.Invoke();
            }
        }

        // Remove response notes that have passed and invoke event handler
        private void HandleResponseNotes(double time)
        {
            while(_responseNotes.Count > 0 && _responseNotes.Peek().Time + _currentSong.GoodTimeMs / 1000 < time)
            {
                _responseNotes.Dequeue();
                OnMiss?.Invoke();
                OnResponseNote?.Invoke(this, Judgement.Miss);
            }
        }

        private void HandleSongEvent(double time)
        {
            while(_songEvents.Count > 0 && _songEvents.Peek().Time < time)
            {
                var e = _songEvents.Dequeue();
                Debug.Log("SongEvent: " + e.Type);
                OnSongEvent?.Invoke(this, e.Type);
            }
        }

        private void HandleBeat(double time)
        {
            if (time > _nextBeat * CurrentSong.TimePerBeat) {
                OnBeat?.Invoke(this, _nextBeat);
                _nextBeat += 1;
            }
        }

        // Check if the user hit the notes on-time
        private void OnBeatInput(InputAction.CallbackContext context)
        {
            if (_responseNotes.Count <= 0) {
                return;
            }

            double accuracy = Math.Abs(RhythmEngine.GetCurrentAudioTime() - _responseNotes.Peek().Time) * 1000;

            if(accuracy < _currentSong.PerfectTimeMs)
            {
                _responseNotes.Dequeue();
                OnPerfect?.Invoke();
                OnResponseNote?.Invoke(this, Judgement.Perfect);
            } else if (accuracy < _currentSong.GoodTimeMs)
            {
                _responseNotes.Dequeue();
                OnGood?.Invoke();
                OnResponseNote?.Invoke(this, Judgement.Good);
            } else if (accuracy < _currentSong.MissTimeMs) {
                // Close enough to hit a note, but not good
                _responseNotes.Dequeue();
                OnMiss?.Invoke();
                OnResponseNote?.Invoke(this, Judgement.Miss);
            } else {
                // No nearby note
                OnMiss?.Invoke();
                OnResponseNote?.Invoke(this, Judgement.NoNote);
            }
        }

        // End the level if the song is over
        private void EndLevel()
        {
            GameLifecycleManager.Instance.EndLevel();
        }
        
        public double TimeUntilCallNote()
        {
            if (_callNotes.Count == 0)
            {
                return -1;
            }
            else
            {
                return _callNotes.Peek().Time - RhythmEngine.GetCurrentAudioTime();
            }
        }
    }
}
