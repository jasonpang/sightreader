using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Chord : List<Note>, MeasureElement
    {
        public bool IsArpeggiated { get; set; }

        public override string ToString()
        {
            return $"Chord <{String.Join(", ", (char[])(ConvertAll(x => x.Pitch.Step).ToArray()))}>";
        }
    }
}
