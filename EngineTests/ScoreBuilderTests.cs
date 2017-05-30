using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine;
using Engine.Builder;
using System.IO;
using System.Xml;
using Engine.Errors;
using System.Collections.Generic;
using System.Linq;

namespace EngineTests
{
    [TestClass]
    public class ScoreBuilderTests
    {
        [TestMethod]
        public void DeserializeMusicXml_Succeeds_With_Valid_Xml()
        {
            var builder = new ScoreBuilder(File.OpenRead(@"Assets\MusicXml\header.xml"));
            var score = builder.BuildScore();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMusicXmlDocumentException))]
        public void DeserializeMusicXml_Fails_With_Invalid_Xml()
        {
            var builder = new ScoreBuilder(File.OpenRead(@"Assets\MusicXml\invalid.xml"));
            var score = builder.BuildScore();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void DeserializeMusicXml_Fails_With_Non_Existent_Xml()
        {
            var builder = new ScoreBuilder(File.OpenRead(@"Assets\MusicXml\this-file-does-not-exist-48946293.xml"));
        }

        [TestMethod]
        public void GetScoreInfo_CompleteMusicXml()
        {
            var builder = new ScoreBuilder(File.OpenRead(@"Assets\MusicXml\header.xml"));
            var deserializedMusicXml = builder.DeserializeMusicXml(builder.MusicXmlDocumentStream);
            var scoreInfo = builder.GetScoreInfo(deserializedMusicXml);
            Assert.AreEqual("work title work number", scoreInfo.Work);
            Assert.AreEqual("movement title movement number", scoreInfo.Movement);
            CollectionAssert.AreEquivalent(new string[] { "composer", "poet" }, scoreInfo.Creators.Keys.ToArray());
            CollectionAssert.AreEquivalent(new string[] { "composer", "poet" }, scoreInfo.Creators.Values.ToArray());
            CollectionAssert.AreEquivalent(new string[] { "credit words 1", "credit words 2", "credit words 3" }, scoreInfo.Credits.ToArray());
        }

        [TestMethod]
        public void GetScoreInfo_MissingFields()
        {
            var builder = new ScoreBuilder(File.OpenRead(@"Assets\MusicXml\header-missing.xml"));
            var deserializedMusicXml = builder.DeserializeMusicXml(builder.MusicXmlDocumentStream);
            var scoreInfo = builder.GetScoreInfo(deserializedMusicXml);
            Assert.AreEqual("work number", scoreInfo.Work);
            CollectionAssert.AreEquivalent(new string[] { }, scoreInfo.Creators.Keys.ToArray());
            CollectionAssert.AreEquivalent(new string[] { }, scoreInfo.Creators.Values.ToArray());
            CollectionAssert.AreEquivalent(new string[] { }, scoreInfo.Credits.ToArray());
        }

        [TestMethod]
        public void GetScoreParts_TwoPartLists()
        {
            var builder = new ScoreBuilder(File.OpenRead(@"C:\Users\Jason\Downloads\xmlsamples\SchbAvMaSample.xml"));
            var deserializedMusicXml = builder.DeserializeMusicXml(builder.MusicXmlDocumentStream);
            var scoreParts = builder.GetScoreParts(deserializedMusicXml);
        }

        [TestMethod]
        public void GetScorePartMeasures_OutOfOrder_MusicXml_Measures()
        {
            // TODO: Test out of order MusicXML measures
        }
    }
}
