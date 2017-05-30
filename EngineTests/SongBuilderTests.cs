using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine;
using Engine.Builder;
using System.IO;

namespace EngineTests
{
    [TestClass]
    public class SongBuilderTests
    {
        [TestMethod]
        public void TestCreateFromMusicXml()
        {
            var score = SongBuilder.CreateFromMusicXml(File.OpenRead(@"Assets\MusicXml\mozart-piano-sonata.xml"));
        }
    }
}
