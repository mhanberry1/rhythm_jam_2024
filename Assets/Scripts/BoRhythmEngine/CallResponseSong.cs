using System;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmJam
{
/// <summary>
/// A song with a call-response structure.
/// </summary>
[CreateAssetMenu(fileName = "CallResponseSong", menuName = "BoRhythmEngine/Songs/CallResponseSong")]
public class CallResponseSong : Song
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

    public MidiTrack CallMidiTrackToLoad;
    public MidiTrack ResponseMidiTrackToLoad;

    [HideInInspector]
    public double TimePerBeat => 60d / BaseBpm;

    public int TimeToBeatNum(double time)
    {
        return (int)Math.Round(time / TimePerBeat);
    }

}

}
