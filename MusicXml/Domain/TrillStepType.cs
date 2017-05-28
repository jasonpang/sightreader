using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicXml.Domain
{
    /// <summary>
    /// The trill-step type describes the alternating note of trills and mordents for playback, relative to the current note.
    /// </summary>
    public enum TrillStepType
    {
        Whole,
        Half,
        Unison
    }
}
