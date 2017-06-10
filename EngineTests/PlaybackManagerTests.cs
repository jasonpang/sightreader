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
using Engine.Playback;

namespace EngineTests
{
    [TestClass]
    public class PlaybackManagerTests
    {
        [TestMethod]
        public void Playback_SlursThatShouldBeTies_SameMeasure()
        {
            var builder = new ScoreBuilder(File.OpenRead(@"Assets\MusicXml\tied-slur-same-measure.xml"));
            var score = builder.BuildScore();
            var playback = new PlaybackManager(null, score);

            var rightHand = Midi.Pitch.C7;
            var leftHand = Midi.Pitch.A0;
            
            var answer1 = playback.GetCorrectedPitch(rightHand).ToArray();
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.C4
            }, answer1);

            var answer2 = playback.GetCorrectedPitch(rightHand).ToArray();
            CollectionAssert.AreEqual(new[]
            {
                Midi.Pitch.D3
            }, answer2);
        }

        [TestMethod]
        public void Playback_SlursThatShouldBeTies_PreviousMeasure()
        {
            var builder = new ScoreBuilder(File.OpenRead(@"Assets\MusicXml\tied-slur-previous-measure.xml"));
            var score = builder.BuildScore();
            var playback = new PlaybackManager(null, score);

            var rightHand = Midi.Pitch.C7;
            var leftHand = Midi.Pitch.A0;
            
            var answer1 = playback.GetCorrectedPitch(rightHand).ToArray();
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.C4
            }, answer1);

            var answer2 = playback.GetCorrectedPitch(rightHand).ToArray();
            CollectionAssert.AreEqual(new[]
            {
                Midi.Pitch.D3
            }, answer2);
        }

        [TestMethod]
        public void Playback_NoGameNoLifeExcerpt()
        {
            var builder = new ScoreBuilder(File.OpenRead(@"Assets\MusicXml\no-game-no-life-excerpt.xml"));
            var score = builder.BuildScore();
            var playback = new PlaybackManager(null, score);

            var rightHand = Midi.Pitch.C7;
            var leftHand = Midi.Pitch.A0;

            // Measure 1
            //   Right Hand
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.E5,
                    Midi.Pitch.GSharp5
            }, playback.GetCorrectedPitch(rightHand).ToArray());
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.DSharp5
            }, playback.GetCorrectedPitch(rightHand).ToArray());
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.E5
            }, playback.GetCorrectedPitch(rightHand).ToArray());
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.FSharp5
            }, playback.GetCorrectedPitch(rightHand).ToArray());
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.B5
            }, playback.GetCorrectedPitch(rightHand).ToArray());
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.A5
            }, playback.GetCorrectedPitch(rightHand).ToArray());

            // Measure 1
            //   Left Hand
            CollectionAssert.AreEqual(new[]
           {
                    Midi.Pitch.CSharp4,
            }, playback.GetCorrectedPitch(leftHand).ToArray());
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.GSharp4
            }, playback.GetCorrectedPitch(leftHand).ToArray());

            // Measure 2
            //   Right Hand
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.CSharp5,
                    Midi.Pitch.GSharp5
            }, playback.GetCorrectedPitch(rightHand).ToArray());
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.CSharp5
            }, playback.GetCorrectedPitch(rightHand).ToArray());
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.E5
            }, playback.GetCorrectedPitch(rightHand).ToArray());
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.FSharp5
            }, playback.GetCorrectedPitch(rightHand).ToArray());
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.B5
            }, playback.GetCorrectedPitch(rightHand).ToArray());
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.A5
            }, playback.GetCorrectedPitch(rightHand).ToArray());

            // Measure 2
            //   Left Hand
            var answer = playback.GetCorrectedPitch(leftHand).ToArray();
            CollectionAssert.AreEqual(new[]
           {
                    Midi.Pitch.A3,
            }, answer);
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.E4
            }, playback.GetCorrectedPitch(leftHand).ToArray());
            CollectionAssert.AreEqual(new[]
            {
                    Midi.Pitch.A4
            }, playback.GetCorrectedPitch(leftHand).ToArray());
        }
    }
}
