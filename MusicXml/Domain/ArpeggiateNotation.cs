using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicXml.Domain
{
    public class ArpeggiateNotation
    {
        public int Number { get; internal set; } = 0;

        public UpDownDirection Direction { get; internal set; }
    }
}
