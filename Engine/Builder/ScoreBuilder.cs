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
            return score;
        }

        public ScoreInfo GetScoreInfo(scorepartwise rawScore)
        {
            var info = new ScoreInfo();
            info.Work = new[] { rawScore.work?.worktitle, rawScore.work?.worknumber }.JoinIgnoreNullOrEmpty();
            info.Movement = new[] { rawScore.movementtitle, rawScore.movementnumber }.JoinIgnoreNullOrEmpty();
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
    }
}
