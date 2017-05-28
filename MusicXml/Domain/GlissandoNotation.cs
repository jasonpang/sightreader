using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicXml.Domain
{
    public class GlissandoNotation
    {
        public StartStopType Type { get; set; } = StartStopType.Start;

        public int Number { get; set; }
    }
}
