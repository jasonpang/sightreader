using System.Collections.Generic;

namespace MusicXml.Domain
{
	public class Note
	{
		internal Note()
		{
			Type = string.Empty;
			Duration = -1;
			Voice = -1;
			Staff = -1;
			IsChord = false;
		}

		public string Type { get; internal set; }
		
		public int Voice { get; internal set; }

        /// <summary>
        /// Duration is a positive number specified in division units. This is the intended duration vs. notated duration (for instance, swing eighths vs. even eighths, or differences in dotted notes in Baroque-era music). Differences in duration specific to an interpretation or performance should use the note element's attack and release attributes.
        /// </summary>
		public int Duration { get; internal set; }

		public Lyric Lyric { get; internal set; }
		
		public Pitch Pitch { get; internal set; }

		public int Staff { get; internal set; }

        /// <summary>
        /// The chord element indicates that this note is an additional chord tone with the preceding note.
        /// </summary>
		public bool IsChord { get; internal set; }

        /// <summary>
        /// The rest element indicates notated rests or silences.
        /// </summary>
		public bool IsRest { get; internal set; }

        public Notations Notations { get; internal set; }

        /// <summary>
        /// The tie element indicates that a tie begins or ends with this note. The tie element indicates sound; the tied element indicates notation.
        /// </summary>
        public Tie Tie { get; set; }

        public bool IsGrace { get; set; }

        public bool IsDotted { get; set; }

        public string Accidental { get; set; }
	}
}
