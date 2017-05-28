using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicXml.Domain
{
    public class HorizontalTurn
    {
        public StartNoteType StartNote { get; set; }

        public TrillStepType TrillStep { get; set; }

        public TwoNoteTurnType TwoNoteTurn { get; set; }
    }
}
