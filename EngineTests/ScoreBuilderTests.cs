using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine;
using Engine.Builder;
using System.IO;
using System.Xml;
using Engine.Errors;
using System.Collections.Generic;
using System.Linq;
using Engine.Models;

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
            var scoreInfo = builder.GetScoreInfo();
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
            var scoreInfo = builder.GetScoreInfo();
            Assert.AreEqual("work number", scoreInfo.Work);
            CollectionAssert.AreEquivalent(new string[] { }, scoreInfo.Creators.Keys.ToArray());
            CollectionAssert.AreEquivalent(new string[] { }, scoreInfo.Creators.Values.ToArray());
            CollectionAssert.AreEquivalent(new string[] { }, scoreInfo.Credits.ToArray());
        }

        [TestMethod]
        public void GetScorePartMeasures_OutOfOrder_MusicXml_Measures()
        {
            var builder = new ScoreBuilder(File.OpenRead(@"Assets\MusicXml\measures-out-of-order.xml"));
            var score = builder.BuildScore();
            Assert.AreEqual(1, score.Parts.Count);
            Assert.AreEqual(3, score.Parts[0].Measures.Count);
            Assert.AreEqual(new Pitch("C", 0, 1), (score.Parts[0].Measures[0].Elements[0].GroupElements[0] as Note).Pitch);
            Assert.AreEqual(new Pitch("C", 0, 2), (score.Parts[0].Measures[1].Elements[0].GroupElements[0] as Note).Pitch);
            Assert.AreEqual(new Pitch("C", 0, 3), (score.Parts[0].Measures[2].Elements[0].GroupElements[0] as Note).Pitch);
        }

        [TestMethod]
        public void BuildScore_HelloWorld()
        {
            var builder = new ScoreBuilder(File.OpenRead(@"Assets\MusicXml\hello-world.xml"));
            var score = builder.BuildScore();
            Assert.AreEqual(1, score.Parts.Count);
            Assert.AreEqual(2, score.Parts[0].Measures.Count);
            Assert.AreEqual(1, score.Parts[0].Measures[0].Elements.Count);
            Assert.AreEqual(2, score.Parts[0].Measures[1].Elements.Count);
        }

        [TestMethod]
        public void BuildScore_NoGameNoLifeExcerpt()
        {
            var builder = new ScoreBuilder(File.OpenRead(@"Assets\MusicXml\no-game-no-life-excerpt.xml"));
            var score = builder.BuildScore();
            Assert.AreEqual(1, score.Parts.Count);
            Assert.AreEqual(5, score.Parts[0].Measures.Count);

            // Measure 1
            Assert.AreEqual(8, score.Parts[0].Measures[0].Elements.Count);

            // Measure 2
            Assert.AreEqual(8, score.Parts[0].Measures[1].Elements.Count);

            // Measure 3
            Assert.AreEqual(8, score.Parts[0].Measures[2].Elements.Count);

            // Measure 4
            Assert.AreEqual(8, score.Parts[0].Measures[3].Elements.Count);

            // Measure 5
            Assert.AreEqual(10, score.Parts[0].Measures[4].Elements.Count);

            {
                var pitches = new List<Pitch>();
                pitches.Add(new Pitch("B", 0, 4));
                pitches.Add(new Pitch("C", 1, 5));
                pitches.Add(new Pitch("F", 1, 5));
                pitches.Add(new Pitch("A", 0, 3));
                var correctPitches = score.Parts[0].Measures[4].Elements[0].GroupElements.Select(note => ((Note)note).Pitch);
                CollectionAssert.AreEqual(pitches, correctPitches.ToList());
            }

            {
                var pitches = new List<Pitch>();
                pitches.Add(new Pitch("E", 0, 4));
                var correctPitches = score.Parts[0].Measures[4].Elements[1].GroupElements.Select(note => ((Note)note).Pitch);
                CollectionAssert.AreEqual(pitches, correctPitches.ToList());
            }

            {
                var pitches = new List<Pitch>();
                pitches.Add(new Pitch("A", 0, 4));
                var correctPitches = score.Parts[0].Measures[4].Elements[2].GroupElements.Select(note => ((Note)note).Pitch);
                CollectionAssert.AreEqual(pitches, correctPitches.ToList());
            }

            {
                var pitches = new List<Pitch>();
                pitches.Add(new Pitch("E", 0, 5));
                var correctPitches = score.Parts[0].Measures[4].Elements[3].GroupElements.Select(note => ((Note)note).Pitch);
                CollectionAssert.AreEqual(pitches, correctPitches.ToList());
            }

            {
                var pitches = new List<Pitch>();
                pitches.Add(new Pitch("D", 1, 5));
                var correctPitches = score.Parts[0].Measures[4].Elements[4].GroupElements.Select(note => ((Note)note).Pitch);
                CollectionAssert.AreEqual(pitches, correctPitches.ToList());
            }

            {
                var pitches = new List<Pitch>();
                pitches.Add(new Pitch("B", 0, 4));
                pitches.Add(new Pitch("A", 0, 4));
                var correctPitches = score.Parts[0].Measures[4].Elements[5].GroupElements.Select(note => ((Note)note).Pitch);
                CollectionAssert.AreEqual(pitches, correctPitches.ToList());
            }

            {
                var pitches = new List<Pitch>();
                pitches.Add(new Pitch("E", 0, 4));
                var correctPitches = score.Parts[0].Measures[4].Elements[6].GroupElements.Select(note => ((Note)note).Pitch);
                CollectionAssert.AreEqual(pitches, correctPitches.ToList());
            }

            {
                var pitches = new List<Pitch>();
                pitches.Add(new Pitch("A", 0, 3));
                var correctPitches = score.Parts[0].Measures[4].Elements[7].GroupElements.Select(note => ((Note)note).Pitch);
                CollectionAssert.AreEqual(pitches, correctPitches.ToList());
            }

            {
                var pitches = new List<Pitch>();
                pitches.Add(new Pitch("F", 1, 5));
                var correctPitches = score.Parts[0].Measures[4].Elements[8].GroupElements.Select(note => ((Note)note).Pitch);
                CollectionAssert.AreEqual(pitches, correctPitches.ToList());
            }

            {
                var pitches = new List<Pitch>();
                pitches.Add(new Pitch("G", 1, 5));
                var correctPitches = score.Parts[0].Measures[4].Elements[9].GroupElements.Select(note => ((Note)note).Pitch);
                CollectionAssert.AreEqual(pitches, correctPitches.ToList());
            }
        }
    }
}
