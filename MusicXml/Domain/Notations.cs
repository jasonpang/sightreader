using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicXml.Domain
{
    public class Notations
    {
        public ArpeggiateNotation Arpeggiate { get; set; } = new ArpeggiateNotation();

        public GlissandoNotation Glissando { get; set; } = new GlissandoNotation();

        public Ornaments Ornaments { get; set; } = new Ornaments();

        public Tied Tied { get; set; }

        public bool IsTupliet { get; set; }
    }
}
