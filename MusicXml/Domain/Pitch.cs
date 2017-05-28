using System;

namespace MusicXml.Domain
{
    /// <summary>
    /// Pitch is represented as a combination of the step of the diatonic scale, the chromatic alteration, and the octave.
    /// </summary>
	public class Pitch
	{
		internal Pitch()
		{
			Alter = 0;
			Octave = 0;
			Step = new Char();
		}

        /// <summary>
        /// The alter element represents chromatic alteration in number of semitones (e.g., -1 for flat, 1 for sharp). Decimal values like 0.5 (quarter tone sharp) are used for microtones. The octave element is represented by the numbers 0 to 9, where 4 indicates the octave started by middle C.  In the first example below, notice an accidental element is used for the third note, rather than the alter element, because the pitch is not altered from the the pitch designated to that staff position by the key signature.
        /// </summary>
		public int Alter { get; internal set; }

        /// <summary>
        /// Octaves are represented by the numbers 0 to 9, where 4 indicates the octave started by middle C.
        /// </summary>
		public int Octave { get; internal set; }

        /// <summary>
        /// The step type represents a step of the diatonic scale, represented using the English letters A through G.
        /// </summary>
		public char Step { get; internal set; }
	}
}