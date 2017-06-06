using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Note : ISpanningMeasureElement
    {
        /// <summary>
        /// In a Chord, some notes can be arpeggiated while others are not.
        /// Arpeggiated chord notes are played broken while non-arpeggiated chord notes are played simultaneously.
        /// This tracks which Chord notes are arpeggiated.
        /// </summary>
        public bool IsArpeggiatedUp { get; set; }
        public bool IsArpeggiatedDown { get; set; }

        public bool IsChordTone { get; set; }

        public bool IsRest { get; set; }

        public bool IsGrace { get; set; }

        public bool IsTiedStart { get; set; }

        public bool IsTiedStop { get; set; }

        public Pitch Pitch { get; set; } = new Pitch();

        public int Duration { get; set; }

        public int Staff { get; set; }

        public int Voice { get; set; }

        public bool IsSilent { get; set; }

        public String Type { get; set; }

        public static bool operator ==(Note a, Note b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.IsArpeggiatedUp == b.IsArpeggiatedUp &&
                a.IsArpeggiatedDown == b.IsArpeggiatedDown &&
                a.IsChordTone == b.IsChordTone &&
                a.IsRest == b.IsRest &&
                a.IsGrace == b.IsGrace &&
                a.IsTiedStart == b.IsTiedStart &&
                a.IsTiedStop == b.IsTiedStop &&
                a.Pitch == b.Pitch &&
                a.Duration == b.Duration &&
                a.Staff == b.Staff &&
                a.Voice == b.Voice &&
                a.IsSilent == b.IsSilent &&
                a.Type == b.Type;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            
            var p = obj as Note;
            if ((System.Object)p == null)
            {
                return false;
            }

            return IsArpeggiatedUp == p.IsArpeggiatedUp &&
                IsArpeggiatedDown == p.IsArpeggiatedDown &&
                IsChordTone == p.IsChordTone &&
                IsRest == p.IsRest &&
                IsGrace == p.IsGrace &&
                IsTiedStart == p.IsTiedStart &&
                IsTiedStop == p.IsTiedStop &&
                Pitch == p.Pitch &&
                Duration == p.Duration &&
                Staff == p.Staff &&
                Voice == p.Voice &&
                IsSilent == p.IsSilent &&
                Type == p.Type;
        }


        public static bool operator !=(Note a, Note b)
        {
            return !(a == b);
        }
    }
}
