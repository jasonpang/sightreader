using Engine.Builder.MusicXml;
using Engine.Errors;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Engine.Utils.Extensions;

namespace Engine.Builder
{
    public class ScoreBuilder
    {
        public Stream MusicXmlDocumentStream { get; }
        public scorepartwise RawScore { get; private set; }

        public ScoreBuilder(Stream musicXmlDocumentStream)
        {
            MusicXmlDocumentStream = musicXmlDocumentStream;
        }

        public Score BuildScore()
        {
            var rawScore = DeserializeMusicXml(MusicXmlDocumentStream);
            return BuildScoreFromRaw(rawScore);
        }

        public scorepartwise DeserializeMusicXml(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(scorepartwise));
            try
            {
                scorepartwise score = (scorepartwise)serializer.Deserialize(stream);
                RawScore = score;
                return score;
            } 
            catch (Exception ex) when (ex.InnerException is XmlException)
            {
                throw new InvalidMusicXmlDocumentException(ex.InnerException as XmlException);
            }
        }

        public Score BuildScoreFromRaw(scorepartwise rawScore)
        {
            var score = new Score();
            score.Info = GetScoreInfo(rawScore);
            score.Parts = GetScoreParts(rawScore);
            return score;
        }

        public ScoreInfo GetScoreInfo(scorepartwise rawScore)
        {
            var info = new ScoreInfo();
            info.Work = new[] { rawScore.work?.worktitle, rawScore.work?.worknumber }.JoinIgnoreNullOrEmpty(" ");
            info.Movement = new[] { rawScore.movementtitle, rawScore.movementnumber }.JoinIgnoreNullOrEmpty(" ");
            foreach (var creator in rawScore.identification?.creator ?? new dynamic[] { })
            {
                info.Creators.Add(creator.type, creator.Value);
            }
            foreach (var creditNode in rawScore.credit ?? new dynamic[] { })
            {
                foreach (dynamic creditText in creditNode?.Items)
                {
                    dynamic credits = info.Credits;
                    credits.Add(creditText?.Value);
                }
            }
            return info;
        }

        public IList<ScorePart> GetScoreParts(scorepartwise rawScore)
        {
            var parts = new List<ScorePart>();
            foreach (var part in rawScore.part)
            {
                if (part.id == null)
                {
                    throw new InvalidMusicXmlDocumentException(null, "This invalid MusicXML document does not contain a part ID.");
                }
                parts.Add(new ScorePart()
                {
                    Id = part.id,
                    Measures = GetScorePartMeasures(part)
                });
            }
            return parts;
        }

        public IList<Measure> GetScorePartMeasures(scorepartwisePart rawPart)
        {
            /*
             * Although MusicXML measures should be in order, there's no guarantee that the data is listed in ascending order.
             * In case the MusicXML file has measures out of order, our insertion into a sorted dictionary will ensure the final retrieval is sorted.
             */
            var measuresMap = new SortedDictionary<int, Measure>();
            foreach (var rawMeasure in rawPart.measure)
            {
                var measure = new Measure();
                measure.Elements = GetScorePartMeasureElements(rawMeasure);
                var measureNumber = Convert.ToInt32(rawMeasure.number);
                if (measuresMap.ContainsKey(measureNumber))
                {
                    throw new InvalidMusicXmlDocumentException(null, $"<measure number='{measureNumber}'> is illegally repeated in this MusicXML document.");
                }
                measuresMap.Add(measureNumber, measure);
            }
            return measuresMap.Values.ToList();
        }

        public IList<IMeasureElement> GetScorePartMeasureElements(scorepartwisePartMeasure rawMeasure)
        {
            var elements = new List<IMeasureElement>();
            return elements;
        }
    }
}
