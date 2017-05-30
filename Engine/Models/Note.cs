using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Note : SpanningMeasureElement
    {
        /// <summary>
        /// In a Chord, some notes can be arpeggiated while others are not.
        /// Arpeggiated chord notes are played broken while non-arpeggiated chord notes are played simultaneously.
        /// This tracks which Chord notes are arpeggiated.
        /// </summary>
        public bool IsArpeggiated { get; set; }

        public Pitch Pitch { get; set; }
    }
}
