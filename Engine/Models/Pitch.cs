using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Pitch
    {
        public int Alter { get; set; }
        public int Octave { get; set; }
        public string Step { get; set; }

        public Pitch()
        {
            Step = "";
        }

        public Pitch(string step, int alter, int octave)
        {
            Step = step;
            Alter = alter;
            Octave = octave;
        }

        public static bool operator ==(Pitch a, Pitch b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            
            return a.Alter == b.Alter &&
                a.Octave == b.Octave &&
                a.Step == b.Step;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var p = obj as Pitch;
            if ((System.Object)p == null)
            {
                return false;
            }

            return Alter == p.Alter &&
                Octave == p.Octave &&
                Step == p.Step;
         }

        public static bool operator !=(Pitch a, Pitch b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            var midiNote = new Midi.Note(Step[0], Alter);
            return $"Pitch <{midiNote.PitchInOctave(Octave).ToString()}>";
        }
    }
}
