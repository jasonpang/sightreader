using MusicXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class SongProcessor
    {
        public SightReader SightReader { get; }
        public SongProcessor(SightReader sightReader)
        {
            SightReader = sightReader;
        }
        public Song Process(String filePath)
        {
            var song = new Song(filePath);
            var score = MusicXmlParser.GetScore(filePath);
            int a = 1;
            return song;            
        }
    }
}
