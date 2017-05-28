using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicXml.Domain
{
    public class TrillOrnament
    {
        public StartNoteType StartNote { get; set; }

        /// <summary>
        /// The trill-step attribute describes the alternating note of trills and mordents for playback, relative to the current note.
        /// </summary>
        public TrillStepType TrillStep { get; set; }

        /// <summary>
        /// The two-note-turn attribute describes the ending notes of trills and mordents for playback, relative to the current note.
        /// </summary>
        public TwoNoteTurnType TwoNoteTurn { get; set; }
    }
}
