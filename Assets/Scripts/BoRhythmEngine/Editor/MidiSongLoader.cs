using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace RhythmJam
{

[CustomEditor(typeof(CallResponseSong))]
public class MidiSongLoader : Editor
{
    private CallResponseSong _song;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _song = (CallResponseSong)target;

        EditorGUILayout.Space();
    
        if (GUILayout.Button("LoadMidi"))
        {
            if (_song.CallMidiTrackToLoad != null) {
                Debug.Log("Loading notes from midi");
                LoadMidi(_song.CallNotes, _song.CallMidiTrackToLoad);
            } else {
                Debug.Log("Nothing to load");
                return;
            }
            if (_song.ResponseMidiTrackToLoad != null) {
                LoadMidi(_song.ResponseNotes, _song.ResponseMidiTrackToLoad);
            } else if (_song.CallMidiTrackToLoad != null) {
                // If no response midi specified, just copy call notes with delay
                AutoPopulateResponses();
            }
        }
    }

    void LoadMidi(List<CallResponseNote> notes, MidiTrack track) {
        notes.Clear();
        foreach (var time in track.NoteTimes) {
            var note = new CallResponseNote();
            note.Time = time * 60d / _song.BaseBpm;
            notes.Add(note);
        }
    }

    void AutoPopulateResponses() {
        _song.ResponseNotes.Clear();
        foreach (var callNote in _song.CallNotes) {
            var note = new CallResponseNote();
            note.Time = callNote.Time + _song.CallResponseInterval * _song.TimePerBeat;
            _song.ResponseNotes.Add(note);
        }
    }

}

}
