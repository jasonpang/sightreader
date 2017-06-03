using Engine.Builder.MusicXml;
using Engine.Errors;
using Engine.Models;
using Engine.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Builder
{
    public class MeasureBuilder
    {
        public scorepartwisePartMeasure RawMeasure { get; set; }
        public SortedDictionary<int, IList<IMeasureElement>> MeasureElements { get; set; }
        public SortedDictionary<int, Measure> Measures { get; set; }
        public int Clock { get; set; }

        public MeasureBuilder(scorepartwisePartMeasure rawMeasure, SortedDictionary<int, Measure> measures)
        {
            RawMeasure = rawMeasure;
            Measures = measures;
            MeasureElements = new SortedDictionary<int, IList<IMeasureElement>>();
        }

        public Measure BuildMeasure()
        {
            var measure = new Measure();
            ProcessRawMeasureElements();
            measure.Elements = MeasureElements.Values.SelectMany(x => x).ToList();
            return measure;
        }

        public void ProcessRawMeasureElements()
        {
            foreach (var element in RawMeasure.Items)
            {
                if (element.GetType() == typeof(backup))
                {
                    BuildMeasureElement_Backup((backup)element);
                }
                else if (element.GetType() == typeof(forward))
                {
                    BuildMeasureElement_Forward((forward)element);
                }
            }
        }

        public void BuildMeasureElement_Backup(backup element)
        {
            if (!element.duration.IsInteger())
            {
                throw new InvalidMusicXmlDocumentException(null, $"Backup duration {element.duration.ToString()} should be an integer.");
            }
            Clock -= (int)element.duration;
        }

        public void BuildMeasureElement_Forward(forward element)
        {
            if (!element.duration.IsInteger())
            {
                throw new InvalidMusicXmlDocumentException(null, $"Forward duration {element.duration.ToString()} should be an integer.");
            }
            Clock += (int)element.duration;
        }
    }
}
