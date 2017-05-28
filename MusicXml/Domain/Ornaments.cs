using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicXml.Domain
{
    /// <summary>
    /// Ornaments can be any of several types, followed optionally by accidentals. The accidental-mark element's content is represented the same as an accidental element, but with a different name to reflect the different musical meaning.
    /// </summary>
    public class Ornaments
    {
        public bool IsTurn { get; set; }
        public bool IsTrill { get; set; }
        public bool IsTrillStart { get; set; }
        public bool IsTrillStop { get; set; }

        /*
        /// <summary>
        /// The delayed-inverted-turn element indicates an inverted turn that is delayed until the end of the current note.
        /// </summary>
        public HorizontalTurn DelayedInvertedTurn { get; set; }

        /// <summary>
        /// The delayed-turn element indicates a normal turn that is delayed until the end of the current note.
        /// </summary>
        public HorizontalTurn DelayedTurn { get; set; }

        public Mordent InvertedMordent { get; set; }

        public HorizontalTurn InvertedTurn { get; set; }

        public Mordent Mordent { get; set; }

        /// <summary>
        /// The trill-mark element represents the trill-mark symbol.
        /// </summary>
        public TrillOrnament Trill { get; set; }

        /// <summary>
        /// The turn element is the normal turn shape which goes up then down.
        /// </summary>
        public HorizontalTurn Turn { get; set; }

        /// <summary>
        /// Wavy lines are one way to indicate trills. When used with a measure element, they should always have type="continue" set.
        /// </summary>
        public WavyLineOrnament WavyLine { get; set; }
        */
    }
}
