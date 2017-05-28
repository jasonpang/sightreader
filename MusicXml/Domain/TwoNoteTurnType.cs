using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicXml.Domain
{
    /// <summary>
    /// The two-note-turn type describes the ending notes of trills and mordents for playback, relative to the current note.
    /// </summary>
    public enum TwoNoteTurnType
    {
        Whole,
        Half,
        None
    }
}
