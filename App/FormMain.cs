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

        private void Input_NoteOn(NoteOnMessage msg)
        {

        }

        private void TextBox_Measure_TextChanged(object sender, EventArgs e)
        {
            TrySetMeasure();
        }

        private void TrySetMeasure()
        {
            try
            {
                var number = Convert.ToInt32(TextBox_Measure.Text);
                SightReader.SetMeasure(Math.Max(0, number - 1));
            }
            catch (Exception ex)
            {

            }
        }

        private void SightReader_MidiInputChanged(object sender, EventArgs e)
        {
            StatusBar_Input.Text = SightReader.Input.Name;
            SightReader.Input.NoteOn += Input_NoteOn;
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
            StatusBar_FileName.Text = Path.GetFileName(SightReader.FilePath);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            PopulateMidiInputOutputs();
            PopulateModes();
            ChooseDefaultMidiInputOutput();
            if (SightReader.Input != null)
            {
                SightReader.SetMode(SightReaderMode.Passthrough);
            }
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
                if (input.Name == "KAWAI USB MIDI")
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

        private void LoadDefaultMidiFile()
        {
            //SightReader.SetFile(@"C:\Users\Jason\Documents\OCR Sheet Music Converted\No_Game_No_Life_-_This_Game_-_Piano.xml");
            SightReader.SetFile(@"C:\Users\Jason\Downloads\Interstellar_-_First_Step (1)\lg-166842060.xml");            
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SightReader.Dispose();
        }

        private void MainMenu_Debug_SendOutput_Click(object sender, EventArgs e)
        {
            var outputDevice = SightReader.Output;

            PlayTestTripleSensor(outputDevice);
        }

        private void PlayTestTripleSensor(OutputDevice outputDevice)
        {
            int velocity = 65;
            int sleep = 500;
            int times = 3;
            Task.Factory.StartNew(() =>
            {
                outputDevice.SilenceAllNotes();
                outputDevice.SendControlChange(Channel.Channel1, Midi.Control.SustainPedal, 0);

                PlayTestNote(outputDevice, velocity, 127/2, times, sleep);
                //Thread.Sleep(1000);
                //PlayTestNote(outputDevice, velocity, 127/2, times, sleep);
                //Thread.Sleep(1000);
                //PlayTestNote(outputDevice, velocity, 127, times, sleep);
            });
        }

        private void PlayTestNote(OutputDevice device, int velocityOn, int velocityOff, int times, int duration)
        {
            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < times; i++)
                {
                    device.SendNoteOn(Channel.Channel1, Midi.Pitch.C4, velocityOn);
                    Thread.Sleep(duration);
                    //device.SendNoteOff(Channel.Channel1, Midi.Pitch.C4, velocityOff);
                    //Thread.Sleep(duration);
                }
            });
        }

        private void PlayPedaledScale(OutputDevice outputDevice)
        {
            Task.Factory.StartNew(() =>
            {
                outputDevice.SilenceAllNotes();
                outputDevice.SendControlChange(Channel.Channel1, Midi.Control.SustainPedal, 0);
                outputDevice.SendPitchBend(Channel.Channel1, 8192);
                // Play C, E, G in half second intervals.
                outputDevice.SendNoteOn(Channel.Channel1, Midi.Pitch.C4, 50);
                Thread.Sleep(500);
                outputDevice.SendNoteOn(Channel.Channel1, Midi.Pitch.E4, 50);
                Thread.Sleep(500);
                outputDevice.SendNoteOn(Channel.Channel1, Midi.Pitch.G4, 50);
                Thread.Sleep(500);

                // Now apply the sustain pedal.
                outputDevice.SendControlChange(Channel.Channel1, Midi.Control.SustainPedal, 127);

                // Now release the C chord notes, but they should keep ringing because of the sustain
                // pedal.
                outputDevice.SendNoteOff(Channel.Channel1, Midi.Pitch.C4, 50);
                outputDevice.SendNoteOff(Channel.Channel1, Midi.Pitch.E4, 50);
                outputDevice.SendNoteOff(Channel.Channel1, Midi.Pitch.G4, 50);
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
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Home)
            {
                SightReader.PlaybackManager.Reset();
            }
        }

        private void MainMenu_Debug_Break_Click(object sender, EventArgs e)
        {
            Debugger.Break();
        }

        private void Button_ChangeMeasure_Click(object sender, EventArgs e)
        {
            TrySetMeasure();
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            int voice = GetVoiceForKey(e.KeyCode);

            if (voice == -1)
            {
                return;
            }


        }

        private int GetVoiceForKey(Keys key)
        {
            if (key == Keys.Q ||
                key == Keys.W ||
                key == Keys.E ||
                key == Keys.R ||
                key == Keys.T ||
                key == Keys.Y ||
                key == Keys.U)
            {
                return 2;
            }
            else if (key == Keys.G ||
                key == Keys.H ||
                key == Keys.J ||
                key == Keys.K ||
                key == Keys.L ||
                key == Keys.OemSemicolon ||
                key == Keys.OemQuotes)
            {
                return 1;
            }
            else return -1;
        }
    }
}
