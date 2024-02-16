using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmJam
{

public class MidiTrack : ScriptableObject
{
    // public MidiParser.MidiTrack Track;
    public List<double> NoteTimes = new();
}

}
