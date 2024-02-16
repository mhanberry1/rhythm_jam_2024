using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmJam
{

/// <summary>
/// A class that can be used to watch the notes of a CallResponse song and trigger events when notes come up.
/// </summary>
[AddComponentMenu("Rhythm Engine/Extensions/Call-Response Announcer")]
public class CallResponseAnnouncer : MonoBehaviour
{
    public BoRhythmEngineCore RhythmEngine;
     
    /// <summary>
    /// Called when a call note is played.
    /// </summary>
    public event Action<CallResponseNote> OnCallNotePlayed;

    /// <summary>
    /// Called when a response note is played.
    /// </summary>
    public event Action<CallResponseNote> OnResponseNotePlayed;

    private CallResponseSong _song;

    private int _nextCallNoteIndex = 0;
    private int _nextResponseNoteIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (RhythmEngine.Song == null && RhythmEngine.ManualMode)
        {
            Debug.LogError("You should call SetSong/SetClip() and InitTime() in Awake() first!");
            return;
        }

        _song = RhythmEngine.Song as CallResponseSong;
        if (_song == null)
        {
            Debug.LogError("CallResponseAnnouncer requires a CallResponseSong to work.");
            return;
        }
    }

    void OnEnable()
    {
        RhythmEngine.OnStartPlaying += OnStartPlaying;
    }

    void OnDisable()
    {
        RhythmEngine.OnStartPlaying -= OnStartPlaying;
    }

    // Update is called once per frame
    void Update()
    {
        if (!RhythmEngine.HasStarted) return;

        var time = RhythmEngine.GetCurrentAudioTime() - RhythmEngine.Song.FirstBeatOffsetInSec;
        if (_nextCallNoteIndex < _song.CallNotes.Count && time >= _song.CallNotes[_nextCallNoteIndex].Time)
        {
            OnCallNotePlayed?.Invoke(_song.CallNotes[_nextCallNoteIndex]);
            _nextCallNoteIndex += 1;
        }
        if (_nextResponseNoteIndex < _song.ResponseNotes.Count && time >= _song.ResponseNotes[_nextResponseNoteIndex].Time)
        {
            OnResponseNotePlayed?.Invoke(_song.ResponseNotes[_nextResponseNoteIndex]);
            _nextResponseNoteIndex += 1;
        }
    }

    // Returns the nearest response note to the given time
    CallResponseNote? GetNearestResponseNote()
    {
        return null;
    }

    // Returns the next closest call note after the given time
    CallResponseNote? GetNextCallNote()
    {
        return _nextCallNoteIndex < _song.CallNotes.Count ? _song.CallNotes[_nextCallNoteIndex] : null;
    }

    void OnStartPlaying()
    {
        var time = RhythmEngine.GetCurrentAudioTime() - RhythmEngine.Song.FirstBeatOffsetInSec;
        // Find next notes from the start
        var callNoteIndex = 0;
        var responseNoteIndex = 0;
        foreach (var note in _song.CallNotes) {
            if (time >= note.Time) {
                callNoteIndex += 1;
            }
        }
        foreach (var note in _song.ResponseNotes) {
            if (time >= note.Time) {
                responseNoteIndex += 1;
            }
        }
        _nextCallNoteIndex = callNoteIndex;
        _nextResponseNoteIndex = responseNoteIndex;
    }
}

}
