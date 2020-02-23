using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleFileBrowser;
using System;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;


// Handles loading a song to the programming plates from a MIDI file.
public class MidiLoader
{
    MarbleMachine marbleMachine;

    public MidiLoader(MarbleMachine marbleMachine)
    {
        this.marbleMachine = marbleMachine;
    }

    public void OpenMidiFileBrowser()
    {
        FileBrowser.ShowLoadDialog(OnMidiFileSelected, null);
    }

    private void OnMidiFileSelected(string path)
    {
        LoadFromMidiFile(path);
    }

    void LoadFromMidiFile(string filePath)
    {
        var pinPositions = new List<List<float>>();
        for (int i = 0; i < marbleMachine.NumChannels; i++)
        {
            pinPositions.Add(new List<float>());
        }

        var midiFile = MidiFile.Read(filePath);
        var tempoMap = midiFile.GetTempoMap();
        foreach (var trackChunk in midiFile.GetTrackChunks())
        {
            using (var noteManager = new NotesManager(trackChunk.Events))
            {
                var notes = noteManager.Notes;
                foreach (var note in notes)
                {
                    var musicalTime = TimeConverter.ConvertTo<BarBeatFractionTimeSpan>(note.Time, tempoMap);
                    var channel = note.NoteNumber - 48;
                    HelperFunctions.Log(channel, (float)(musicalTime.Bars / (float)marbleMachine.NumBars * 360f + musicalTime.Beats / 4f * (360 / (float)marbleMachine.NumBars)));
                    if (channel > pinPositions.Count || channel < 0)
                        continue;
                    pinPositions[channel].Add((float)( musicalTime.Bars / (float)marbleMachine.NumBars * 360f + musicalTime.Beats / 4f * (360 / (float)marbleMachine.NumBars) ));
                }
            }
            /*using (var timedEventManager = new TimedEventsManager(trackChunk.Events))
            {
                foreach (var timedEvent in timedEventManager.Events.GetTimedEventsAndNotes())
                {
                    if (timedEvent is TimedEvent note && note.Event.EventType == MidiEventType.NoteOn)
                    {
                        pinPositions[note.Event.]
                    }
                }
            }*/
        }

        marbleMachine.LoadProgramming(pinPositions);
    }
}
