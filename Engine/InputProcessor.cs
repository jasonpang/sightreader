using Engine.Models;
using Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class InputProcessor
    {
        private SightReader SightReader { get; set; }

        private Song song { get { return SightReader.Song; } }

        private Dictionary<Midi.Pitch, List<Midi.Pitch>> OnOffMap { get; set; } = new Dictionary<Midi.Pitch, List<Midi.Pitch>>();

        public InputProcessor(SightReader sightReader)
        {
            SightReader = sightReader;
            SightReader.ModeChanged += SightReader_ModeChanged;
        }

        private void SightReader_ModeChanged(object sender, EventArgs e)
        {
        }

        public void OnNoteOn(NoteOnMessage note)
        {
            if (SightReader.Mode == SightReaderMode.Passthrough)
            {
                //Debug.WriteLine($"({SightReader.Mode.ToString()}) Note On: {note.ToString()}");
                SightReader.Output.SendNoteOn(note.Channel, note.Pitch, note.Velocity);
            }
            else if (SightReader.Mode == SightReaderMode.Sightreading)
            {
                SightReadOn(note);
            }
        }

        private HandType GetHandType(Midi.Pitch playedPitch)
        {
            if ((byte)playedPitch > 128 / 2)
            {
                return HandType.RightHand;
            } else
            {
                return HandType.LeftHand;
            }
        }

        private void SightReadOn(NoteOnMessage message)
        {
            var hand = GetHandType(message.Pitch);
            Stave stave = null;
            if (hand == HandType.LeftHand)
            {
                stave = song.Parts.First().Staves[1];
            }
            else
            {
                stave = song.Parts.First().Staves[0];
            }

            var measure = stave.Measures[stave.MeasureIndex];
            var measureElement = measure.Elements[measure.MeasureElementIndex];
            if (measureElement is Models.Note)
            {
                var note = (Models.Note)measureElement;
                Debug.WriteLine($"Measure {stave.MeasureIndex}, Element {measure.MeasureElementIndex}: {note.ToString()}");
                SightReader.Output.SendNoteOn(message.Channel, note.Pitch.ToMidiPitch(), message.Velocity);
                var notesOffList = new List<Midi.Pitch>();
                notesOffList.Add(note.Pitch.ToMidiPitch());
                OnOffMap[message.Pitch] = notesOffList;
            }
            else if (measureElement is Models.Chord)
            {
                var chord = (Models.Chord)measureElement;
                var notesOffList = new List<Midi.Pitch>();
                Debug.WriteLine($"Measure {stave.MeasureIndex}, Element {measure.MeasureElementIndex}: {chord.ToString()}");
                foreach (var note in chord.Notes)
                {
                    SightReader.Output.SendNoteOn(message.Channel, note.Pitch.ToMidiPitch(), message.Velocity);
                    notesOffList.Add(note.Pitch.ToMidiPitch());
                }
                OnOffMap[message.Pitch] = notesOffList;
            }
            stave.Measures[stave.MeasureIndex].MeasureElementIndex++;
            if (stave.Measures[stave.MeasureIndex].MeasureElementIndex >= stave.Measures[stave.MeasureIndex].Elements.Count)
            {
                stave.Measures[stave.MeasureIndex].MeasureElementIndex = 0;
                stave.MeasureIndex++;
            }
        }

        private void SightReadOff(NoteOffMessage message)
        {
            if (OnOffMap.ContainsKey(message.Pitch))
            {
                var pitchesToRelease = OnOffMap[message.Pitch];
                foreach (var pitchToRelease in pitchesToRelease)
                {
                    SightReader.Output.SendNoteOff(message.Channel, pitchToRelease, 0);
                }
            }
        }

        public void OnNoteOff(NoteOffMessage note)
        {
            if (SightReader.Mode == SightReaderMode.Passthrough)
            {
                //Debug.WriteLine($"({SightReader.Mode.ToString()}) Note Off: {note.ToString()}");
                SightReader.Output.SendNoteOff(note.Channel, note.Pitch, note.Velocity);
            }
            else if (SightReader.Mode == SightReaderMode.Sightreading)
            {
                SightReadOff(note);
            }
        }

        public void OnControlChange(ControlChangeMessage control)
        {
            //Debug.WriteLine($"({SightReader.Mode.ToString()}) Control Change: {control.ToString()}");
            SightReader.Output.SendControlChange(control.Channel, control.Control, control.Value);
        }
    }
}
