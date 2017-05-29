using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Note : MeasureElement
    {
        public Pitch Pitch { get; set; }

        public int Voice { get; set; }

        /// <summary>
        /// Duration is a positive number specified in division units. This is the intended duration vs. notated duration (for instance, swing eighths vs. even eighths, or differences in dotted notes in Baroque-era music). Differences in duration specific to an interpretation or performance should use the note element's attack and release attributes.
        /// </summary>
        public int Duration { get; internal set; }

        public int Staff { get; internal set; }

        public string Type { get; set; }

        public bool IsArpeggiated { get; set; }

        public bool IsRest { get; set; }

        public override string ToString()
        {
            return $"Note <{Pitch.Step}>";
        }
    }
}
