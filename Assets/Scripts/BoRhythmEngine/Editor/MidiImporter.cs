using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;

namespace RhythmJam
{

[ScriptedImporter(1, "mid")]
public class MidiImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var midiFile = new MidiParser.MidiFile(ctx.assetPath);

        int trackCount = 0;
        int ticksPerQuarterNote = midiFile.TicksPerQuarterNote;
        foreach(var track in midiFile.Tracks)
        {
            var trackAsset = ScriptableObject.CreateInstance<RhythmJam.MidiTrack>();
            foreach(var midiEvent in track.MidiEvents) {
                if (midiEvent.MidiEventType == MidiParser.MidiEventType.MetaEvent) {
                    if (midiEvent.Arg1 == (byte)MidiParser.MetaEventType.Tempo) {
                        Debug.Log("bpm " + (int)midiEvent.Arg2);
                    }
                }
                if (midiEvent.MidiEventType == MidiParser.MidiEventType.NoteOn) {
                    double time = (double)midiEvent.Time / (double)ticksPerQuarterNote;
                    trackAsset.NoteTimes.Add(time);
                }
            }
            ctx.AddObjectToAsset("track" + trackCount, trackAsset);
        }
    }

}

}