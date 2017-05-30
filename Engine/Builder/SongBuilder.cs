using Engine.Builder.MusicXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engine.Builder
{
    public class SongBuilder
    {
        public static scorepartwise CreateFromMusicXml(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(scorepartwise));
            scorepartwise score = (scorepartwise)serializer.Deserialize(stream);
            return score;
        }
    }
}
