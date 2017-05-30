using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Chord : SpanningMeasureElement
    {
        public IList<Note> Notes { get; set; }
    }
}
