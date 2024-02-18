using System;
using UnityEngine;

namespace RhythmJam
{
    /// <summary>
    /// Simple struct containing the data of a call-response note.
    /// </summary>
    [Serializable]
    public struct SongEvent
    {
        public enum EventType
        {
            Divergence
        }

        public double Time;
        public EventType Type;

        public bool Equals(SongEvent other)
        {
            // We use a small epsilon to account for floating point errors.
            return Math.Abs(Time - other.Time) < 0.0001d && Type == other.Type;
        }
    }
}
