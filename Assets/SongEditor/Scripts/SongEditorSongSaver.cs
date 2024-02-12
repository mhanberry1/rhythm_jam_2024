using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using RhythmEngine;

namespace RhythmJam
{
    /// <summary>
    /// Helper class to save a call response song in the editor.
    /// </summary>
    public static class SongEditorSongSaver
    {
        public static void Save(bool[,] notes, CallResponseSong songToSave)
        {
            Debug.Log("Save");
            #if UNITY_EDITOR
            // Since we're only getting the indexes of placed notes, we need to calculate the exact timings of each note.
            var songMinutes = songToSave.Clip.length / 60;
            var barCount = songMinutes * (songToSave.BaseBpm / 4);
            barCount = Mathf.Ceil(barCount);
            int beatCount = (int)barCount * songToSave.BeatsPerBar * songToSave.BeatSubdivisions;
            var timePerBeat = 60d / songToSave.BaseBpm / 4;

            var callNotesToSave = new List<CallResponseNote>();
            var responseNotesToSave = new List<CallResponseNote>();
            for (int beat = 0; beat < beatCount; beat++)
            {
                for (int lane = 0; lane < 2; lane++)
                {
                    if (notes[lane, beat])
                    {
                        var note = new CallResponseNote
                        {
                            // We're adding the song offset here because the calculation is done on the absolute timings of the notes.
                            // Like if we have a 10s intro to the song, we don't want the first note be at 0s, but at 10s.
                            Time = BeatToTime(beat, timePerBeat) + songToSave.FirstBeatOffsetInSec
                        };
                        switch (lane) {
                            case 0:
                                callNotesToSave.Add(note);
                                break;
                            case 1:
                                responseNotesToSave.Add(note);
                                break;
                        }
                    }
                }
            }

            // This isn't needed, but I like to have the notes sorted by time in the inspector.
            callNotesToSave = callNotesToSave.OrderBy(note => note.Time).ToList();
            responseNotesToSave = responseNotesToSave.OrderBy(note => note.Time).ToList();

            // Apply changes...
            songToSave.CallNotes = callNotesToSave;
            songToSave.ResponseNotes = responseNotesToSave;

            // ...and actually save the changes to the asset file
            EditorUtility.SetDirty(songToSave);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            #endif
        }

        private static double BeatToTime(int beat, double timePerBeat) => beat * timePerBeat;
    }

}
