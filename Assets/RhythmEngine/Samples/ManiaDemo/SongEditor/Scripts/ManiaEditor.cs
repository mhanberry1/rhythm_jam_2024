using System;
using UnityEngine;
using UnityEngine.UI;

namespace RhythmEngine.Examples
{
    /// <summary>
    /// Main class for the mania editor.
    /// Manages the song, the beat grid, and the notes.
    /// </summary>
    public class ManiaEditor : MonoBehaviour
    {
        [SerializeField] private RhythmEngineCore RhythmEngine;
        [SerializeField] private SimpleManiaSong SongToEdit;

        [Space]
        [SerializeField] private Transform BeatsParent; // Parent for the beat visuals
        [SerializeField] private Transform MajorBeatPrefab, MinorBeatPrefab;

        // Visual spacing between beats
        // It's public because it's needed in the NotePlacer
        public float BeatSpacing = 1f;

        [Space]
        [SerializeField] private Transform NotePrefab;
        [SerializeField] private float NoteOnBeatHeight = 0.33f; // The offset from the middle of a beaet to place a note
        [SerializeField] private float[] LanePositions = { -2.25f, -0.75f, 0.75f, 2.25f }; // We need the lane positions to correctly place the notes

        [Space]
        [SerializeField] private float BeatsKeyHeight = -4f; // The mininimum height of the beats parent

        [Space]
        [SerializeField] private Slider SongPositionSlider; // For visual representation of the song position

        // Shorthands for the song properties
        private double SongOffset => SongToEdit.FirstBeatOffsetInSec;
        private double SongLength => SongToEdit.Clip.length;

        private const int LaneCount = 4;

        private Transform[,] _noteGrid;

        private double _timePerBeat;
        private float _rawBeatCount; // Needed to properly match the beat parent y position to the current song position
        private int _maxBeat; // Needed to not place notes after the song ends

        private void Awake()
        {
            // Since we're in Manual Mode, we need to set the song manually
            RhythmEngine.SetSong(SongToEdit);
            RhythmEngine.InitTime();
            RhythmEngine.SetStartTime(SongOffset);
            // We aren't Play()'ing the song immediately.

            // The sample song has 17 bars, and 16 beats per bar.
            var songMinutes = SongToEdit.Clip.length / 60;
            var barCount = songMinutes * (SongToEdit.BaseBpm / 4);
            var rawBarCount = barCount;
            barCount = Mathf.Ceil(barCount); // We're ceiling the bar count in case there are only a couple of beats in the last bar.
            int beatCount = (int)barCount * 16;
            _rawBeatCount = rawBarCount * 16;
            _maxBeat = beatCount;
            _timePerBeat = 60d / SongToEdit.BaseBpm / 4;

            CreateBeatVisuals(beatCount);
            LoadNotesFromSong(beatCount);

            SongPositionSlider.value = 0;

            // We're pausing the song in the beginning so that the user can choose when to start editing the song.
            Pause();

            // We're setting the beats parent position to the beginning of the song.
            // This will also automatically apply the first beat offset.
            UpdateBeatsPosition(0f);
        }

        /// <summary>
        /// Spawns all the beat lines.
        /// </summary>
        private void CreateBeatVisuals(int beatCount)
        {
            for (int i = 0; i < beatCount; i++)
            {
                var prefabToSpawn = i % 4 == 0 ? MajorBeatPrefab : MinorBeatPrefab;
                var beat = Instantiate(prefabToSpawn, BeatsParent);
                beat.name = $"Beat {i}, Bar{Math.Floor(i / 16d)}";
                beat.localPosition = new Vector3(0f, i * BeatSpacing, 0f);
            }
        }

        /// <summary>
        /// Spawns already existing notes from the song.
        /// </summary>
        private void LoadNotesFromSong(int beatCount)
        {
            _noteGrid = new Transform[LaneCount, beatCount];

            foreach (var note in SongToEdit.Notes)
            {
                var gridPosition = TimeToGridPosition(note.Time, note.Lane);
                CreateNewNote(gridPosition);
            }
        }

        /// <summary>
        /// Creates a new note at the given position.
        /// </summary>
        private void CreateNewNote(Vector2Int gridPosition)
        {
            var noteTransform = Instantiate(NotePrefab, BeatsParent);
            noteTransform.localPosition = GridPositionToNotePosition(gridPosition);

            _noteGrid[gridPosition.x, gridPosition.y] = noteTransform;
        }

        /// <summary>
        /// Returns the world position of a note from a grid position.
        /// </summary>
        private Vector3 GridPositionToNotePosition(Vector2Int gridPosition)
        {
            var x = LanePositions[gridPosition.x];
            var y = gridPosition.y * BeatSpacing + NoteOnBeatHeight;

            return new Vector3(x, y, 0f);
        }

        /// <summary>
        /// Returns a grid position from a time and lane.
        /// Needed to spawn notes from the song and to place new notes.
        /// </summary>
        private Vector2Int TimeToGridPosition(double time, int lane)
        {
            var beat = Math.Round(time / _timePerBeat);

            return new Vector2Int(lane, (int)beat);
        }

        private void Update()
        {
            // In the update loop we're checking for user input and updating the editor visuals.

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (RhythmEngine.IsPaused)
                {
                    UnPause();
                }
                else
                {
                    Pause();
                }
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                ScrollSnap(-1f);
            }
            else if (Input.mouseScrollDelta.y > 0)
            {
                ScrollSnap(1f);
            }

            // We want to update the song position when the song is playing.
            if (!RhythmEngine.IsPaused)
            {
                var time = RhythmEngine.GetCurrentAudioTime();
                UpdateVisuals(time);
            }
        }

        /// <summary>
        /// Helper function to update both the slider and the beats parent position.
        /// </summary>
        private void UpdateVisuals(double time)
        {
            var sliderValue = (float)(time / SongLength);
            SongPositionSlider.value = sliderValue;

            UpdateBeatsPosition(time);
        }

        /// <summary>
        /// Updates the Beats Parent position to match the current song position.
        /// </summary>
        private void UpdateBeatsPosition(double time)
        {
            // This is the max y position that the beats parent can have.
            var maxPosition = BeatsKeyHeight - BeatSpacing * _rawBeatCount;

            // We need to adjust the time to account for the first beat offset.
            var adjustedTime = time - SongOffset;
            var songFraction = (float)(adjustedTime / SongLength);

            // Then we lerp the beats parent position to match the current song position.
            var beatsPosition = Mathf.Lerp(BeatsKeyHeight, maxPosition, songFraction);
            BeatsParent.localPosition = new Vector3(0f, beatsPosition, 0f);
        }

        /// <summary>
        /// Snaps the song position to the next/previous beat.
        /// </summary>
        private void ScrollSnap(float scrollDir)
        {
            var pauseTime = RhythmEngine.SourceStartTime; // We want to save the time we paused the song at.

            if (!RhythmEngine.IsPaused)
            {
                // You might want to not pause the song when scrolling.
                // This is just my preference.
                Pause();
            }

            var scrolledTime = pauseTime + -scrollDir * _timePerBeat; // Current time +- 1 beat.
            var snappedTime = SnapTime(scrolledTime);

            // We need to offset the time by the song offset, because the snapped time is absolute.
            snappedTime += SongOffset;

            // Then we clamp the snapped time to the song length and make sure it's not negative.
            snappedTime = Math.Max(Math.Max(SongOffset, 0d), Math.Min(SongLength, snappedTime));

            // Manually update the slider and beats parent.
            UpdateVisuals(snappedTime);

            RhythmEngine.SetStartTime(snappedTime);
        }

        /// <summary>
        /// Snaps time to the nearest beat. Doesn't account for the first beat offset.
        /// </summary>
        private double SnapTime(double time)
        {
            var snapped = Math.Round(time / _timePerBeat) * _timePerBeat;
            return snapped;
        }

        private void Pause()
        {
            RhythmEngine.Pause();
        }

        private void UnPause()
        {
            // We need to clamp the time to the song length and make sure it's not negative.
            var time = Math.Max(Math.Max(SongOffset, 0d), Math.Min(SongLength, RhythmEngine.SourceStartTime));

            RhythmEngine.SetStartTime(time);
            RhythmEngine.Unpause();
        }

        /// <summary>
        /// Activates or deactivates the note at the given grid position.
        /// This is used by the NotePlacer to place and remove notes near the mouse.
        /// </summary>
        public void ToggleNote(Vector2Int gridPosition)
        {
            var lane = gridPosition.x;
            var beat = gridPosition.y;

            if (beat < 0 || beat > _maxBeat) return; // We don't want to go outside of the song length.

            var note = _noteGrid[lane, beat];
            if (note != null)
            {
                Destroy(note.gameObject);
                _noteGrid[lane, beat] = null;
            }
            else
            {
                CreateNewNote(gridPosition);
            }
        }

        /// <summary>
        /// Called from a button. Converts the internal note grid to a bool array and saves it to a file.
        /// </summary>
        public void Save()
        {
            var notes = new bool[_noteGrid.GetLength(0), _noteGrid.GetLength(1)];

            for (int y = 0; y < _noteGrid.GetLength(1); y++)
            {
                for (int x = 0; x < _noteGrid.GetLength(0); x++)
                {
                    notes[x, y] = _noteGrid[x, y] != null;
                }
            }

            ManiaEditorSongSaver.Save(notes, SongToEdit);
        }
    }
}
