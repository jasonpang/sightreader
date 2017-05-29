using Engine.Models;
using Midi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class SightReader : IDisposable
    {
        public InputDevice Input { get; private set; }

        public OutputDevice Output { get; private set; }

        public SightReaderMode Mode { get; private set; }

        public InputProcessor InputProcessor { get; private set; }
        public SongProcessor SongProcessor { get; private set; }

        public Song Song { get; private set; }

        public SightReader()
        {
            InputProcessor = new InputProcessor(this);
            SongProcessor = new SongProcessor(this);
        }

        public ReadOnlyCollection<InputDevice> GetMidiInputDevices()
        {
            return InputDevice.InstalledDevices;
        }

        public ReadOnlyCollection<OutputDevice> GetMidiOutputDevices()
        {
            return OutputDevice.InstalledDevices;
        }

        public void SetMidiInput(InputDevice device)
        {
            CloseMidiInput();

            Trace.WriteLine($"Setting new MIDI input '{device.Name}'.");
            Input = device;
            Trace.WriteLine($"Installing input event handlers.");
            Input.NoteOn += InputProcessor.OnNoteOn;
            Input.NoteOff += InputProcessor.OnNoteOff;
            Input.ControlChange += InputProcessor.OnControlChange;
            Trace.WriteLine($"Opening MIDI input.");
            Input.Open();
            Input.StartReceiving(null);

            MidiInputChanged?.Invoke(this, null);
        }

        public void SetMeasure(int number)
        {
            if (number > 0 && number <= Song.Parts.First().Staves.First().Measures.Count)
            {
                foreach (var part in Song.Parts)
                {
                    foreach (var stave in part.Staves)
                    {
                        stave.MeasureIndex = number - 1;
                        foreach (var measure in stave.Measures)
                        {
                            measure.MeasureElementIndex = 0;
                        }
                    }
                }
            }
        }

        public void CloseMidiInput()
        {
            if (Input != null)
            {
                Trace.WriteLine($"Removing all event handlers on input '{Input.Name}'.");
                Input.RemoveAllEventHandlers();
                // If we're still receiving, we have to stop it first
                if (Input.IsReceiving)
                {
                    Trace.WriteLine($"Stopping input events on input '{Input.Name}'.");
                    Input.StopReceiving();
                }
                // Close an existing open input
                if (Input.IsOpen)
                {
                    Trace.WriteLine($"Closing input '{Input.Name}'.");
                    Input.Close();
                }
            }
        }

        public void SetMidiOutput(OutputDevice device)
        {
            CloseMidiOutput();

            Trace.WriteLine($"Setting new MIDI output '{device.Name}'.");
            Output = device;
            Trace.WriteLine($"Opening MIDI output.");
            Output.Open();
            Output.SilenceAllNotes();

            MidiOutputChanged?.Invoke(this, null);
        }

        private void CloseMidiOutput()
        {
            if (Output != null)
            {
                if (Output.IsOpen)
                {
                    Trace.WriteLine($"Silencing all notes on output '{Output.Name}'.");
                    Output.SilenceAllNotes();
                    Trace.WriteLine($"Closing output '{Output.Name}'.");
                    Output.Close();
                }
            }
        }

        public void SetMode(SightReaderMode mode)
        {
            Mode = mode;
            ModeChanged?.Invoke(this, null);
        }

        public event EventHandler MidiInputChanged;
        public event EventHandler MidiOutputChanged;
        public event EventHandler ModeChanged;
        public event EventHandler SongChanged;

        public void SetFile(string filePath)
        {
            Song = SongProcessor.Process(filePath);
            SetMode(SightReaderMode.Sightreading);
            SongChanged?.Invoke(this, null);
        }

        public void Dispose()
        {
            CloseMidiInput();
            CloseMidiOutput();
        }
    }
}
