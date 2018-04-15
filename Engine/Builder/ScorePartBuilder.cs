using Engine.Builder.MusicXml;
using Engine.Errors;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Builder
{
    public class ScorePartBuilder
    {
        public scorepartwisePart RawPart { get; set; }

        /*
         * Although MusicXML measures should be in order, there's no guarantee that the data is listed in ascending order.
         * In case the MusicXML file has measures out of order, our insertion into a sorted dictionary will ensure the final retrieval is sorted.
         */
        public SortedDictionary<int, Measure> Measures { get; set; }

        public ScorePartBuilder(scorepartwisePart rawPart)
        {
            RawPart = rawPart;
            Measures = new SortedDictionary<int, Measure>();
        }

        public ScorePart BuildPart()
        {
            return new ScorePart()
            {
                Id = RawPart.id,
                Measures = GetScorePartMeasures()
            };
        }

        private bool IsInvalidMeasure(string rawMeasure)
        {
            foreach (char c in rawMeasure)
            {
                if (c < '0' || c > '9')
                    return true;
            }

            return false;
        }
        
        public IList<Measure> GetScorePartMeasures()
        {
            foreach (var rawMeasure in RawPart.measure)
            {
                var measure = new MeasureBuilder(rawMeasure, Measures).BuildMeasure();
                if (IsInvalidMeasure(rawMeasure.number))
                {
                    continue;
                }
                var measureNumber = Convert.ToInt32(rawMeasure.number);
                if (Measures.ContainsKey(measureNumber))
                {
                    throw new InvalidMusicXmlDocumentException(null, $"<measure number='{measureNumber}'> is illegally repeated in this MusicXML document.");
                }
                Measures.Add(measureNumber, measure);
            }
            return Measures.Values.ToList();
        }
    }
}
