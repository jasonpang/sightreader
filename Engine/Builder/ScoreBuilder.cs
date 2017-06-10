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
        public scorepartwise RawScore { get; set; }

        public ScoreBuilder(Stream musicXmlDocumentStream)
        {
            RawScore = ScoreBuilder.DeserializeMusicXml(musicXmlDocumentStream);
        } 

        public ScoreBuilder(scorepartwise rawScore)
        {
            RawScore = rawScore;
        }

        public static scorepartwise DeserializeMusicXml(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(scorepartwise));
            try
            {
                scorepartwise score = (scorepartwise)serializer.Deserialize(stream);
                return score;
            }
            catch (Exception ex) when (ex.InnerException is XmlException)
            {
                throw new InvalidMusicXmlDocumentException(ex.InnerException as XmlException);
            }
        }

        public Score BuildScore()
        {
            var score = new Score();
            score.Info = GetScoreInfo();
            score.Parts = GetScoreParts();
            FixSameSlursToTies(score);
            return score;
        }

        public void FixSameSlursToTies(Score score)
        {

        }

        public ScoreInfo GetScoreInfo()
        {
            var info = new ScoreInfo();
            info.Work = new[] { RawScore.work?.worktitle, RawScore.work?.worknumber }.JoinIgnoreNullOrEmpty(" ");
            info.Movement = new[] { RawScore.movementtitle, RawScore.movementnumber }.JoinIgnoreNullOrEmpty(" ");
            foreach (var creator in RawScore.identification?.creator ?? new dynamic[] { })
            {
                info.Creators.Add(creator.type, creator.Value);
            }
            foreach (var creditNode in RawScore.credit ?? new dynamic[] { })
            {
                foreach (dynamic creditText in creditNode?.Items)
                {
                    dynamic credits = info.Credits;
                    credits.Add(creditText?.Value);
                }
            }
            return info;
        }

        public IList<ScorePart> GetScoreParts()
        {
            var parts = new List<ScorePart>();
            foreach (var part in RawScore.part)
            {
                if (part.id == null)
                {
                    throw new InvalidMusicXmlDocumentException(null, "This invalid MusicXML document does not contain a part ID.");
                }
                parts.Add(new ScorePartBuilder(part).BuildPart());
            }
            return parts;
        }
    }
}
