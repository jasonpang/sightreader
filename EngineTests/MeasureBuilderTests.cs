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
            Assert.AreEqual(measure.Elements.First().GetType(), typeof(ElementGroup));
            var note = measure.Elements.First().GroupElements.First() as Note;
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
            Assert.AreEqual(measure.Elements.First().GetType(), typeof(ElementGroup));
            var note = measure.Elements.First().GroupElements.First() as Note;
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
            // 4 notes, 1 at clock time 0, 3 at clock time 4, for a total of 2 ElementGroup items
            Assert.AreEqual(2, measure.Elements.Count);
            Assert.AreEqual(((Note)(measure.Elements[0].GroupElements[0])).IsChordTone, false);
            Assert.AreEqual(((Note)(measure.Elements[0].GroupElements[1])).IsChordTone, true);
            Assert.AreEqual(((Note)(measure.Elements[0].GroupElements[2])).IsChordTone, true);
            Assert.AreEqual(((Note)(measure.Elements[1].GroupElements[0])).IsChordTone, false);
        }

        [TestMethod]
        public void BuildMeasure_Note_Rest()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-rest.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(measure.Elements.Count, 1);
            Assert.AreEqual(((Note)(measure.Elements[0].GroupElements[0])).IsRest, true);
        }

        [TestMethod]
        public void BuildMeasure_Note_Grace()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-grace.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(1, measure.Elements.Count);
            Assert.AreEqual(true, ((Note)(measure.Elements[0].GroupElements[0])).IsGrace);
        }

        [TestMethod]
        public void BuildMeasure_Note_Tied_Start()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-tied-start.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(1, measure.Elements.Count);
            Assert.AreEqual(true, ((Note)(measure.Elements[0].GroupElements[0])).IsTiedStart);
        }

        [TestMethod]
        public void BuildMeasure_Note_Tied_Stop()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-tied-stop.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(1, measure.Elements.Count);
            Assert.AreEqual(true, ((Note)(measure.Elements[0].GroupElements[0])).IsTiedStop);
        }

        [TestMethod]
        public void BuildMeasure_Note_Tied_StartStop()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-tied-start-stop.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(1, measure.Elements.Count);
            Assert.AreEqual(true, ((Note)(measure.Elements[0].GroupElements[0])).IsTiedStart);
            Assert.AreEqual(true, ((Note)(measure.Elements[0].GroupElements[0])).IsTiedStop);
        }

        [TestMethod]
        public void BuildMeasure_Note_Arpeggiate_Default()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\note-arpeggiate-default.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(measure.Elements.Count, 1);
            Assert.AreEqual(measure.Elements.First().GetType(), typeof(ElementGroup));
            var note = measure.Elements.First().GroupElements.First() as Note;
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
            Assert.AreEqual(measure.Elements.First().GetType(), typeof(ElementGroup));
            var note = measure.Elements.First().GroupElements.First() as Note;
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
            Assert.AreEqual(measure.Elements.First().GetType(), typeof(ElementGroup));
            var note = measure.Elements.First().GroupElements.First() as Note;
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
            Assert.AreEqual(1, measure.Elements.Count);
            Assert.AreEqual(2, measure.Elements[0].GroupElements.Count);
            Assert.AreEqual(4, builder.Clock);
        }

        [TestMethod]
        public void BuildMeasure_Clock_AdvancePartially_NormalChord()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\clock-chord-normal.xml"));
            builder.ProcessOnly.Add(typeof(note));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(4, builder.Clock);
            Assert.AreEqual(1, measure.Elements.Count);
            Assert.AreEqual(2, measure.Elements[0].GroupElements.Count);
        }

        [TestMethod]
        public void BuildMeasure_TwoStave_Chord()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\two-stave-chord.xml"));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(1, measure.Elements.Count);
            Assert.AreEqual(3, measure.Elements[0].GroupElements.Count);
            Assert.AreEqual(new Pitch("C", 1, 4), (measure.Elements[0].GroupElements[0] as Note).Pitch);
            Assert.AreEqual(new Pitch("E", 0, 5), (measure.Elements[0].GroupElements[1] as Note).Pitch);
            Assert.AreEqual(new Pitch("G", 1, 5), (measure.Elements[0].GroupElements[2] as Note).Pitch);
        }

        [TestMethod]
        public void BuildMeasure_IdenticalNoteBacktrack()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\identical-note-backtrack.xml"));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(1, measure.Elements.Count);
            Assert.AreEqual(1, measure.Elements[0].GroupElements.Count);
            Assert.AreEqual(new Pitch("A", 0, 3), (measure.Elements[0].GroupElements[0] as Note).Pitch);
            Assert.AreEqual(3360, (measure.Elements[0].GroupElements[0] as Note).Duration);
            Assert.AreEqual(3360, builder.Clock);
        }

        [TestMethod]
        public void BuildMeasure_IdenticalNoteBacktrack2()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\identical-note-backtrack-2.xml"));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(1, measure.Elements.Count);
            Assert.AreEqual(2, measure.Elements[0].GroupElements.Count);
            Assert.AreEqual(new Pitch("B", 0, 1), (measure.Elements[0].GroupElements[0] as Note).Pitch);
            Assert.AreEqual(new Pitch("B", 0, 2), (measure.Elements[0].GroupElements[1] as Note).Pitch);
            Assert.AreEqual(3360, (measure.Elements[0].GroupElements[0] as Note).Duration);
            Assert.AreEqual(3360, builder.Clock);
        }

        [TestMethod]
        public void BuildMeasure_Clock_Complex()
        {
            var builder = new MeasureBuilder(File.OpenRead(@"Assets\MusicXml\Measures\clock-complex.xml"));
            var measure = builder.BuildMeasure();
            Assert.AreEqual(new Pitch("E", 0, 5), (measure.Elements[0].GroupElements[1] as Note).Pitch);
            Assert.AreEqual(new Pitch("E", 0, 6), (measure.Elements[0].GroupElements[2] as Note).Pitch);

            Assert.AreEqual(new Pitch("B", 0, 5), (measure.Elements[1].GroupElements[0] as Note).Pitch);

            Assert.AreEqual(new Pitch("G", 1, 5), (measure.Elements[2].GroupElements[0] as Note).Pitch);

            Assert.AreEqual(new Pitch("E", 0, 6), (measure.Elements[3].GroupElements[0] as Note).Pitch);
            Assert.AreEqual(new Pitch("E", 0, 5), (measure.Elements[3].GroupElements[1] as Note).Pitch);

            Assert.AreEqual(new Pitch("B", 0, 5), (measure.Elements[4].GroupElements[0] as Note).Pitch);

            Assert.AreEqual(new Pitch("G", 1, 5), (measure.Elements[5].GroupElements[0] as Note).Pitch);

            Assert.AreEqual(new Pitch("E", 0, 6), (measure.Elements[6].GroupElements[0] as Note).Pitch);
            Assert.AreEqual(new Pitch("E", 0, 5), (measure.Elements[6].GroupElements[1] as Note).Pitch);

            Assert.AreEqual(new Pitch("B", 0, 5), (measure.Elements[7].GroupElements[0] as Note).Pitch);

            Assert.AreEqual(new Pitch("G", 1, 5), (measure.Elements[8].GroupElements[0] as Note).Pitch);

            Assert.AreEqual(new Pitch("E", 0, 5), (measure.Elements[9].GroupElements[0] as Note).Pitch);
            Assert.AreEqual(new Pitch("E", 0, 6), (measure.Elements[9].GroupElements[1] as Note).Pitch);

            Assert.AreEqual(new Pitch("B", 0, 5), (measure.Elements[10].GroupElements[0] as Note).Pitch);

            Assert.AreEqual(new Pitch("G", 1, 5), (measure.Elements[11].GroupElements[0] as Note).Pitch);

            Assert.AreEqual(new Pitch("F", 1, 5), (measure.Elements[12].GroupElements[0] as Note).Pitch);
            Assert.AreEqual(new Pitch("F", 1, 6), (measure.Elements[12].GroupElements[1] as Note).Pitch);

            Assert.AreEqual(new Pitch("G", 1, 6), (measure.Elements[13].GroupElements[0] as Note).Pitch);
            Assert.AreEqual(new Pitch("G", 1, 5), (measure.Elements[13].GroupElements[1] as Note).Pitch);
        }
    }
}
