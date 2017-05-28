using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicXml.Domain
{
    public class Mordent
    {
        public bool Long { get; set; }

        /// <summary>
        /// The approach and departure attributes are used for compound ornaments, indicating how the beginning and ending of the ornament look relative to the main part of the mordent.
        /// </summary>
        public AboveBelowType Approach { get; set; }

        /// <summary>
        /// The approach and departure attributes are used for compound ornaments, indicating how the beginning and ending of the ornament looks relative to the main part of the mordent.
        /// </summary>
        public AboveBelowType Departure { get; set; }

        public StartNoteType StartNote { get; set; }

        public TrillStepType TrillStep { get; set; }

        public TwoNoteTurnType TwoNoteTurn { get; set; }
    }
}
