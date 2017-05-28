using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicXml.Domain
{
    public class Tuplet
    {
        public StartStopType Type { get; set; }

        public int Number { get; set; }

        public bool IsBracketed { get; set; }
    }
}
