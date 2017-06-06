using Engine.Builder.MusicXml;
using Engine.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils.Extensions
{
    public static class MusicXmlBuilderExtensions
    {
        public static string ToLetterString(this step pitchStep)
        {
            switch (pitchStep)
            {
                case step.A:
                    return "A";
                case step.B:
                    return "B";
                case step.C:
                    return "C";
                case step.D:
                    return "D";
                case step.E:
                    return "E";
                case step.F:
                    return "F";
                case step.G:
                    return "G";
                default:
                    throw new InvalidMusicXmlDocumentException(null, $"Invalid note pitch step {pitchStep.ToString()}");
            }
        }
    }
}
