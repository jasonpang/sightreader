using Engine;
using Engine.Models;
using Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class FormMain : Form
    {
        private SightReader SightReader { get; set; }

        public FormMain()
        {
            InitializeComponent();
            SightReader = new SightReader();
            SightReader.MidiInputChanged += SightReader_MidiInputChanged;
            SightReader.MidiOutputChanged += SightReader_MidiOutputChanged;
            SightReader.ModeChanged += SightReader_ModeChanged;
            SightReader.SongChanged += SightReader_SongChanged;
        }

        private void SightReader_MidiInputChanged(object sender, EventArgs e)
        {
            StatusBar_Input.Text = SightReader.Input.Name;
        }

        private void SightReader_MidiOutputChanged(object sender, EventArgs e)
        {
            StatusBar_Output.Text = SightReader.Output.Name;
        }

        private void SightReader_ModeChanged(object sender, EventArgs e)
        {
            StatusBar_Mode.Text = SightReader.Mode.ToString();
        }

        private void SightReader_SongChanged(object sender, EventArgs e)
        {
            StatusBar_FileName.Text = Path.GetFileName(SightReader.Song.FilePath);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            SightReader.SetMode(SightReaderMode.Passthrough);
            PopulateMidiInputOutputs();
            PopulateModes();
            ChooseDefaultMidiInputOutput();
        }

        private void PopulateMidiInputOutputs()
        {
            var inputs = SightReader.GetMidiInputDevices();
            foreach (var input in inputs)
            {
                var toolStripItem = new ToolStripMenuItem(input.Name, null, (s, e) => { SightReader.SetMidiInput(input); });
                MainMenu_MIDI_Input.DropDownItems.Add(toolStripItem);
            }
            var outputs = SightReader.GetMidiOutputDevices();
            foreach (var output in outputs)
            {
                var toolStripItem = new ToolStripMenuItem(output.Name, null, (s, e) => { SightReader.SetMidiOutput(output); });
                MainMenu_MIDI_Output.DropDownItems.Add(toolStripItem);
            }
        }

        private void PopulateModes()
        {
            foreach (SightReaderMode mode in Enum.GetValues(typeof(SightReaderMode)))
            {
                var toolStripItem = new ToolStripMenuItem(mode.ToString(), null, (s, e) => { SightReader.SetMode(mode); });
                MainMenu_Program_Mode.DropDownItems.Add(toolStripItem);
            }
        }

        private void ChooseDefaultMidiInputOutput()
        {
            var inputs = SightReader.GetMidiInputDevices();
            foreach (var input in inputs)
            {
                if (input.Name == "E-MU XMidi1X1 Tab")
                {
                    SightReader.SetMidiInput(input);
                    break;
                }
            }
            var outputs = SightReader.GetMidiOutputDevices();
            foreach (var output in outputs)
            {
                if (output.Name.ToLower().Contains("virtual"))
                {
                    SightReader.SetMidiOutput(output);
                    break;
                }
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SightReader.Dispose();
        }

        private void MainMenu_Debug_SendOutput_Click(object sender, EventArgs e)
        {
            var outputDevice = SightReader.Output;

            Task.Factory.StartNew(() =>
            {
                outputDevice.SilenceAllNotes();
                outputDevice.SendControlChange(Channel.Channel1, Midi.Control.SustainPedal, 0);
                outputDevice.SendPitchBend(Channel.Channel1, 8192);
                // Play C, E, G in half second intervals.
                outputDevice.SendNoteOn(Channel.Channel1, Pitch.C4, 50);
                Thread.Sleep(500);
                outputDevice.SendNoteOn(Channel.Channel1, Pitch.E4, 50);
                Thread.Sleep(500);
                outputDevice.SendNoteOn(Channel.Channel1, Pitch.G4, 50);
                Thread.Sleep(500);

                // Now apply the sustain pedal.
                outputDevice.SendControlChange(Channel.Channel1, Midi.Control.SustainPedal, 127);

                // Now release the C chord notes, but they should keep ringing because of the sustain
                // pedal.
                outputDevice.SendNoteOff(Channel.Channel1, Pitch.C4, 50);
                outputDevice.SendNoteOff(Channel.Channel1, Pitch.E4, 50);
                outputDevice.SendNoteOff(Channel.Channel1, Pitch.G4, 50);
            });
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            var filesList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (filesList != null && filesList.Length > 0)
            {
                var file = filesList.First();
                Trace.WriteLine($"Processing file '{file}'");
                SightReader.SetFile(file);
            }
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
    }
}
