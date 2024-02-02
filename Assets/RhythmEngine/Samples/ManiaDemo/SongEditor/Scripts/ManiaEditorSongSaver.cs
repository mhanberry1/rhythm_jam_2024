using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace RhythmEngine.Examples
{
    /// <summary>
    /// Helper class to save a mania song in the editor.
    /// </summary>
    public static class ManiaEditorSongSaver
    {
        public static void Save(bool[,] notes, SimpleManiaSong songToSave)
        {
            #if UNITY_EDITOR
            // Since we're only getting the indexes of placed notes, we need to calculate the exact timings of each note.
            var songMinutes = songToSave.Clip.length / 60;
            var barCount = songMinutes * (songToSave.BaseBpm / 4);
            barCount = Mathf.Ceil(barCount);
            int beatCount = (int)barCount * 16;
            var timePerBeat = 60d / songToSave.BaseBpm / 4;

            var notesToSave = new List<SimpleManiaNote>();
            for (int beat = 0; beat < beatCount; beat++)
            {
                for (int lane = 0; lane < 4; lane++)
                {
                    if (notes[lane, beat])
                    {
                        notesToSave.Add(new SimpleManiaNote
                        {
                            Lane = lane,
                            // We're adding the song offset here because the calculation is done on the absolute timings of the notes.
                            // Like if we have a 10s intro to the song, we don't want the first note be at 0s, but at 10s.
                            Time = BeatToTime(beat, timePerBeat) + songToSave.FirstBeatOffsetInSec
                        });
                    }
                }
            }

            // This isn't needed, but I like to have the notes sorted by time in the inspector.
            notesToSave = notesToSave.OrderBy(note => note.Time).ToList();

            // Apply changes...
            songToSave.Notes = notesToSave;

            // ...and actually save the changes to the asset file
            EditorUtility.SetDirty(songToSave);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            #endif
        }

        private static double BeatToTime(int beat, double timePerBeat) => beat * timePerBeat;
    }

}
