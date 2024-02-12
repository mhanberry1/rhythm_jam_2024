using System;
using UnityEngine;

namespace RhythmEngine.Examples
{
    /// <summary>
    /// Simple struct containing the data of a call-response note.
    /// </summary>
    [Serializable]
    public struct CallResponseNote
    {
        [Tooltip("The time in seconds at which to play the note.")]
        public double Time;

        public bool Equals(CallResponseNote other)
        {
            // We use a small epsilon to account for floating point errors.
            return Math.Abs(Time - other.Time) < 0.0001d;
        }
    }
}
