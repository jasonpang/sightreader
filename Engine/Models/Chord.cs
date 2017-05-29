using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Chord : MeasureElement
    {
        public bool IsArpeggiated { get; set; }

        public List<Note> Notes { get; set; } = new List<Note>();

        public override string ToString()
        {
            return $"Chord <{String.Join(", ", (char[])(Notes.ConvertAll(x => x.Pitch.Step).ToArray()))}>";
        }
    }
}
