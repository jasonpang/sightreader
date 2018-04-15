using Engine.Builder;
using Engine.Models;
using Engine.Playback;
using Midi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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

        public string FilePath { get; set; }

        public ScoreBuilder ScoreBuilder { get; set; }

        public PlaybackManager PlaybackManager { get; set; }

        public SightReader()
        {
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
            Trace.WriteLine($"Opening MIDI input.");
            Input.Open();
            Input.StartReceiving(null);

            MidiInputChanged?.Invoke(this, null);
        }

        public void SetMeasure(int number)
        {
            PlaybackManager.SetMeasureIndex(0, number);
            PlaybackManager.SetMeasureIndex(1, number);
            PlaybackManager.SetGroupElementIndex(0, 0);
            PlaybackManager.SetGroupElementIndex(1, 0);
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
            if (FilePath != null && Mode == SightReaderMode.Sightreading)
            {
                ScoreBuilder = new ScoreBuilder(new FileStream(FilePath, FileMode.Open));
                var score = ScoreBuilder.BuildScore();
                PlaybackManager = new PlaybackManager(this, score);
            } else
            {
                PlaybackManager = new PlaybackManager(this, null);
            }
            Input.RemoveAllEventHandlers();
            Input.NoteOn += PlaybackManager.OnNoteOn;
            Input.NoteOff += PlaybackManager.OnNoteOff;
            Input.ControlChange += PlaybackManager.OnControlChange;
            ModeChanged?.Invoke(this, null);
        }

        public event EventHandler MidiInputChanged;
        public event EventHandler MidiOutputChanged;
        public event EventHandler ModeChanged;
        public event EventHandler SongChanged;

        public void SetFile(string filePath)
        {
            FilePath = filePath;
            SongChanged?.Invoke(this, null);
            SetMode(SightReaderMode.Passthrough);
            SetMode(SightReaderMode.Sightreading);
        }

        public void Dispose()
        {
            CloseMidiInput();
            CloseMidiOutput();
        }
    }
}
