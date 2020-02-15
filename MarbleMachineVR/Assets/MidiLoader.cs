using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleFileBrowser;
using System;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;


// Handles loading a song to the programming plates from a MIDI file.
public class MidiLoader : MonoBehaviour
{
    public MarbleMachine MarbleMachine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenMidiFileBrowser()
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
        for (int i = 0; i < MarbleMachine.NumChannels; i++)
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
                    pinPositions[note.NoteNumber - 60].Add((float)( musicalTime.Bars / 16 * 360 + musicalTime.Beats / 4 * (360/16) ));
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
        
    }
}
