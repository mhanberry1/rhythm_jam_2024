using System.Collections.Generic;
using UnityEngine;
using RhythmEngine;

namespace RhythmJam
{
    /// <summary>
    /// A song with a call-response structure.
    /// </summary>
    [CreateAssetMenu(fileName = "CallResponseSong", menuName = "RhythmEngine/Songs/CallResponseSong")]
    public class CallResponseSong : BeatSequencedSong
    {
        [Header("Notes")]
        public List<CallResponseNote> CallNotes = new();
        public List<CallResponseNote> ResponseNotes = new();

        [Tooltip("How many beats in a bar")]
        public int BeatsPerBar = 4;

        [Tooltip("How many subdivisions for aligning notes to off-beats")]
        public int BeatSubdivisions = 4;

        [Tooltip("Number of beats between a call and response")]
        public int CallResponseInterval = 8;

        [Tooltip("Input time offset (in milliseconds) in which a note will be considered 'perfect'")]
        public double PerfectTimeMs = 50;

        [Tooltip("Input time offset (in milliseconds) in which a note will be considered 'good'")]
        public double GoodTimeMs = 100;
    }
}
