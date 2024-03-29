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

    [Header("Events")]
    public List<SongEvent> SongEvents = new();

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

    [Tooltip("Input time offset (in milliseconds) in which a note will be considered close enough to miss")]
    public double MissTimeMs = 200;

    public MidiTrack CallMidiTrackToLoad;
    public MidiTrack ResponseMidiTrackToLoad;

    [HideInInspector]
    public double TimePerBeat => 60d / BaseBpm;

    public int TimeToBeatNum(double time)
    {
        return (int)Math.Round(time / TimePerBeat);
    }

    public int TimeToBarNum(double time) {
        return (int)(time / TimePerBeat / BeatsPerBar);
    }

    public double TimeUntilResponse()
    {
        return CallResponseInterval * TimePerBeat;
    }

    public double DistanceToNearestBeat(double time)
    {
        var lastBeat = (int)(time / TimePerBeat);
        var nextBeat = lastBeat + 1;
        return Math.Min(time - lastBeat * TimePerBeat, nextBeat * TimePerBeat - time);
    }
    
    public double DistanceToNearestNBeat(double time, int n)
    {
        var timePerNBeat = TimePerBeat * n;
        var lastBeat = (int)(time / timePerNBeat);
        var nextBeat = lastBeat + 1;
        return Math.Min(time - lastBeat * timePerNBeat, nextBeat * timePerNBeat - time);
    }

}

}
