using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Pitch
    {
        public char Step { get; set; }
        public int Octave { get; set; }
        public int Alter { get; set; }

        public Midi.Pitch ToMidiPitch()
        {
            return new Midi.Note(Step, Alter).PitchInOctave(Octave);
        }
    }
}
