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
    public class Processor
    {
        private SightReader SightReader { get; set; }
        private Song song;

        public Processor(SightReader sightReader)
        {
            SightReader = sightReader;
            SightReader.ModeChanged += SightReader_ModeChanged;
        }

        private void SightReader_ModeChanged(object sender, EventArgs e)
        {
            if (SightReader.Mode == SightReaderMode.Passthrough)
            {
                song = null;
            }
        }

        public void OnNoteOn(NoteOnMessage note)
        {
            if (SightReader.Mode == SightReaderMode.Passthrough)
            {
                //Debug.WriteLine($"({SightReader.Mode.ToString()}) Note On: {note.ToString()}");
                SightReader.Output.SendNoteOn(note.Channel, note.Pitch, note.Velocity);
            }
        }

        public void OnNoteOff(NoteOffMessage note)
        {
            if (SightReader.Mode == SightReaderMode.Passthrough)
            {
                //Debug.WriteLine($"({SightReader.Mode.ToString()}) Note Off: {note.ToString()}");
                SightReader.Output.SendNoteOff(note.Channel, note.Pitch, note.Velocity);
            }
        }

        public void OnControlChange(ControlChangeMessage control)
        {
            if (SightReader.Mode == SightReaderMode.Passthrough)
            {
                //Debug.WriteLine($"({SightReader.Mode.ToString()}) Control Change: {control.ToString()}");
                SightReader.Output.SendControlChange(control.Channel, control.Control, control.Value);
            }
        }
    }
}
