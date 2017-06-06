using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine;
using Engine.Builder;
using System.IO;
using System.Xml;
using Engine.Errors;
using System.Collections.Generic;
using System.Linq;
using Engine.Builder.MusicXml;
using Engine.Models;

namespace EngineTests
{
    [TestClass]
    public class MeasureBuilderTests
    {
        [TestMethod]
        public void BuildMeasure_Backup()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\backup.xml"));
            builder.ProcessOnly.Add(typeof(backup));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(builder.Clock, new int[] { 1, 2, 3, 4, 5 }.Sum() * -1);
        }

        [TestMethod]
        public void BuildMeasure_Forward()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\forward.xml"));
            builder.ProcessOnly.Add(typeof(forward));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(builder.Clock, new int[] { 1, 2, 3, 4, 5 }.Sum());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMusicXmlDocumentException))]
        public void BuildMeasure_Backup_Fails_With_Decimals()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\backup-decimals.xml"));
            builder.ProcessOnly.Add(typeof(backup));
            var measure = builder.BuildMeasure();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMusicXmlDocumentException))]
        public void BuildMeasure_Forward_Fails_With_Decimals()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\forward-decimals.xml"));
            builder.ProcessOnly.Add(typeof(forward));
            var measure = builder.BuildMeasure();
        }

        [TestMethod]
        public void BuildMeasure_Note_Pitch()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-pitch.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(measure.Elements.Count, 1);
            Assert.AreEqual(measure.Elements.First().GetType(), typeof(Note));
            var note = measure.Elements.First() as Note;
            Assert.AreEqual(note.Pitch.Alter, 1);
            Assert.AreEqual(note.Pitch.Step, "C");
            Assert.AreEqual(note.Pitch.Octave, 4);
        }

        [TestMethod]
        public void BuildMeasure_Note_Duration_Type_Staff_Voice()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-duration-type-staff-voice.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(measure.Elements.Count, 1);
            Assert.AreEqual(measure.Elements.First().GetType(), typeof(Note));
            var note = measure.Elements.First() as Note;
            Assert.AreEqual(4, note.Duration);
            Assert.AreEqual("whole", note.Type);
            Assert.AreEqual(1, note.Staff);
            Assert.AreEqual(3, note.Voice);
        }

        [TestMethod]
        public void BuildMeasure_Note_Chord()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-chord.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(measure.Elements.Count, 4);
            Assert.AreEqual(((Note)(measure.Elements[0])).IsChordTone, false);
            Assert.AreEqual(((Note)(measure.Elements[1])).IsChordTone, true);
            Assert.AreEqual(((Note)(measure.Elements[2])).IsChordTone, true);
            Assert.AreEqual(((Note)(measure.Elements[3])).IsChordTone, false);
        }

        [TestMethod]
        public void BuildMeasure_Note_Rest()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-rest.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(measure.Elements.Count, 1);
            Assert.AreEqual(((Note)(measure.Elements[0])).IsRest, true);
        }

        [TestMethod]
        public void BuildMeasure_Note_Grace()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-grace.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(1, measure.Elements.Count);
            Assert.AreEqual(true, ((Note)(measure.Elements[0])).IsGrace);
        }

        [TestMethod]
        public void BuildMeasure_Note_Tied_Start()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-tied-start.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(1, measure.Elements.Count);
            Assert.AreEqual(true, ((Note)(measure.Elements[0])).IsTiedStart);
        }

        [TestMethod]
        public void BuildMeasure_Note_Tied_Stop()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-tied-stop.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(1, measure.Elements.Count);
            Assert.AreEqual(true, ((Note)(measure.Elements[0])).IsTiedStop);
        }

        [TestMethod]
        public void BuildMeasure_Note_Tied_StartStop()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-tied-start-stop.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(1, measure.Elements.Count);
            Assert.AreEqual(true, ((Note)(measure.Elements[0])).IsTiedStart);
            Assert.AreEqual(true, ((Note)(measure.Elements[0])).IsTiedStop);
        }

        [TestMethod]
        public void BuildMeasure_Note_Arpeggiate_Default()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-arpeggiate-default.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(measure.Elements.Count, 1);
            Assert.AreEqual(measure.Elements.First().GetType(), typeof(Note));
            var note = measure.Elements.First() as Note;
            Assert.AreEqual(note.IsArpeggiatedUp, true);
            Assert.AreEqual(note.IsArpeggiatedDown, false);
        }

        [TestMethod]
        public void BuildMeasure_Note_Arpeggiate_Up()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-arpeggiate-up.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(measure.Elements.Count, 1);
            Assert.AreEqual(measure.Elements.First().GetType(), typeof(Note));
            var note = measure.Elements.First() as Note;
            Assert.AreEqual(note.IsArpeggiatedUp, true);
            Assert.AreEqual(note.IsArpeggiatedDown, false);
        }

        [TestMethod]
        public void BuildMeasure_Note_Arpeggiate_Down()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-arpeggiate-down.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(measure.Elements.Count, 1);
            Assert.AreEqual(measure.Elements.First().GetType(), typeof(Note));
            var note = measure.Elements.First() as Note;
            Assert.AreEqual(note.IsArpeggiatedUp, false);
            Assert.AreEqual(note.IsArpeggiatedDown, true);
        }

        [TestMethod]
        public void BuildMeasure_Clock_Advance_ChordNote()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\clock-chord-none.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(4, builder.Clock);
        }

        [TestMethod]
        public void BuildMeasure_Clock_DoNotAdvance_OneChord()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\clock-chord-one.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(0, builder.Clock);
        }

        [TestMethod]
        public void BuildMeasure_Clock_AdvancePartially_NormalChord()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\clock-chord-normal.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(7, builder.Clock);
        }
    }
}
