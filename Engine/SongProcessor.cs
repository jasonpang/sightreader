using Engine.Models;
using MusicXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            MusicXml.Domain.Score score = MusicXmlParser.GetScore(filePath);
            song.Composer = ProcessComposer(score);
            song.Parts.AddRange(ProcessParts(score));
            return song;            
        }

        public string ProcessComposer(MusicXml.Domain.Score score)
        {
            string composer = score.MovementTitle;
            if (composer == null)
            {
                composer = score.Identification?.Composer;
            }
            return composer;
        }

        public List<Part> ProcessParts(MusicXml.Domain.Score score)
        {
            var parts = new List<Part>();
            foreach (var part in score.Parts)
            {
                var enginePart = new Part();
                SongProcessInfo info = GetSongProcessInfo(part);
                enginePart.Staves.AddRange(ProcessStaves(info, part));
                parts.Add(enginePart);
            }
            return parts;
        }

        public SongProcessInfo GetSongProcessInfo(MusicXml.Domain.Part part)
        {
            var info = new SongProcessInfo();

            foreach (var measure in part.Measures)
            {
                foreach (var element in measure.MeasureElements)
                {
                    if (element.Type == MusicXml.Domain.MeasureElementType.Note)
                    {
                        var note = (MusicXml.Domain.Note)element.Element;
                        if (note.Staff > info.MaxStaves)
                        {
                            info.MaxStaves = note.Staff;
                        }
                    }
                }
            }
            return info;
        }        

        public List<Stave> ProcessStaves(SongProcessInfo info, MusicXml.Domain.Part part)
        {
            // Create an empty array with the maximum number of staves for the song (precalculated by scanning all note staff values to find the maximum staff value)
            var staves = new List<Stave>(info.MaxStaves);

            for (int i = 0; i < info.MaxStaves; i++)
            {
                staves.Add(new Stave());
            }

                // Each measure has various notes, trills, chords, ties that belong on different staffs
                for (int measureIndex = 0; measureIndex < part.Measures.Count; measureIndex++)
            {
                var measure = part.Measures[measureIndex];
                // Process this MusicXML Measure. Out of 2 staffs, maybe the bass staff (2nd) has no notes, so our dictionary only has a mapping to stave 1
                // This method will split a measure so that each stave has its own "copy" of that measure
                Dictionary<int, Measure> measurePerStave = ProcessStavesForMeasure(info, measureIndex, measure);
                for (int i = 0; i < info.MaxStaves; i++)
                {
                    // If there were some measure elements (wrapped up in a Measure object) for this stave, add it to the stave's measures list
                    var processedMeasureForStave = measurePerStave[i];
                    staves[i].Measures.Add(processedMeasureForStave);
                }
            }
            // Our final staves list has a copy of each measure for each stave, though each measure "copy" has different elements depending on the measure elements' target stave
            return staves;
        }

        /// <summary>
        /// Stave -> Measure elements
        /// </summary>
        public Dictionary<int, Measure> ProcessStavesForMeasure(SongProcessInfo info, int measureIndex, MusicXml.Domain.Measure measure)
        {
            var processedMeasuresForStaves = new Dictionary<int, Measure>(info.MaxStaves);
            for (int i = 0; i < info.MaxStaves; i++)
            {
                var measureElementsForStaveNumber = measure.MeasureElements.Where((measureElement) =>
                {
                    return (measureElement.Type == MusicXml.Domain.MeasureElementType.Note && ((MusicXml.Domain.Note)measureElement.Element).Staff == i + 1) ||
                           (measureElement.Type != MusicXml.Domain.MeasureElementType.Note);
                }).ToList();
                var measureForStaveNumber = ProcessMeasureForTargetStave(measureIndex, measureElementsForStaveNumber);
                processedMeasuresForStaves.Add(i, measureForStaveNumber);
            }            
            return processedMeasuresForStaves;
        }

        public Measure ProcessMeasureForTargetStave(int measureIndex, List<MusicXml.Domain.MeasureElement> measureElements)
        {
            var ActualMeasureNumber = measureIndex + 1;
            Measure engineMeasure = new Measure();
            var engineMeasureElements = new SortedDictionary<int, List<MeasureElement>>();
            int clock = 0;
            for (int measureElementIndex = 0; measureElementIndex < measureElements.Count; measureElementIndex++)
            {
                var ActualMeasureElementIndex = measureElementIndex + 1;
                var measureElement = measureElements[measureElementIndex];
                MusicXml.Domain.MeasureElement nextMeasureElement = null;
                if (measureElementIndex < measureElements.Count - 1)
                {
                    nextMeasureElement = measureElements[measureElementIndex + 1];
                }

                if (measureElement.Type == MusicXml.Domain.MeasureElementType.Note)
                {
                    var note = (MusicXml.Domain.Note)measureElement.Element;
                    if (note.IsChord)
                    {
                        /**
                         * Don't advance the clock on chord notes, just add it to the current position's notes.
                         * 
                         * Find the last added note/Chord.
                         */
                        if (!engineMeasureElements.ContainsKey(clock))
                        {
                            engineMeasureElements[clock] = new List<MeasureElement>();
                            engineMeasureElements[clock].Add(ProcessNote(note));
                            clock = GetAdvancedClock(engineMeasureElements, nextMeasureElement, note, clock);
                        }
                        else
                        {
                            var lastNoteOrChordAtPosition = engineMeasureElements[clock].FindLast(engineMeasureElement => engineMeasureElement.GetType() == typeof(Note) || engineMeasureElement.GetType() == typeof(Chord));
                            if (lastNoteOrChordAtPosition.GetType() == typeof(Note))
                            {
                                /**
                                    * We found the last note we're supposed to turn into a Chord!
                                    * We're probably on the 2nd note of the Chord right now.
                                    * Remove the first note and replace it with a formal Chord object.
                                    */
                                engineMeasureElements[clock].Remove(lastNoteOrChordAtPosition);
                                var chord = new Chord();
                                chord.Add(ProcessNote(note));
                                engineMeasureElements[clock].Add(chord);
                                clock = GetAdvancedClock(engineMeasureElements, nextMeasureElement, note, clock);
                            }
                            else if (lastNoteOrChordAtPosition.GetType() == typeof(Chord))
                            {
                                /**
                                    * We're probably continuing an existing Chord, and we're likely on our 3rd+ note.
                                    * Add to the chord.
                                    */
                                ((Chord)lastNoteOrChordAtPosition).Add(ProcessNote(note));
                                clock = GetAdvancedClock(engineMeasureElements, nextMeasureElement, note, clock);
                            }
                        }
                    }
                    else if (note.Notations != null && note.Notations.Ornaments.IsTrill)
                    {
                        var trill = new Trill();
                        trill.StartNote = ProcessNote(note);
                        engineMeasureElements[clock].Add(trill);
                        clock = GetAdvancedClock(engineMeasureElements, nextMeasureElement, note, clock);
                    }
                    else if (note.Tie != null && note.Tie.IsStop)
                    {
                        // Ignore all ending/middle ties
                        // But advance clock unless next note is chord
                        clock = GetAdvancedClock(engineMeasureElements, nextMeasureElement, note, clock);
                    }
                    else if (!note.IsRest)
                    {
                        if (!engineMeasureElements.ContainsKey(clock))
                        {
                            engineMeasureElements[clock] = new List<MeasureElement>();
                        }
                        engineMeasureElements[clock].Add(ProcessNote(note));
                        clock = GetAdvancedClock(engineMeasureElements, nextMeasureElement, note, clock);
                    }
                    else if (note.IsRest)
                    {
                        clock = GetAdvancedClock(engineMeasureElements, nextMeasureElement, note, clock);
                    }
                }
                else if (measureElement.Type == MusicXml.Domain.MeasureElementType.Backup)
                {
                    var backup = (MusicXml.Domain.Backup)measureElement.Element;
                    clock -= backup.Duration;
                }
                else if (measureElement.Type == MusicXml.Domain.MeasureElementType.Forward)
                {
                    var forward = (MusicXml.Domain.Forward)measureElement.Element;
                    clock += forward.Duration;
                }
            }
            List<MeasureElement> myMeasureElements = engineMeasureElements.Values.SelectMany(c => c).ToList();
            engineMeasure.AddRange(myMeasureElements);
            return engineMeasure;
        }

        public Note ProcessNote(MusicXml.Domain.Note note)
        {
            var engineNote = new Note();
            engineNote.Duration = note.Duration;
            engineNote.Pitch = new Pitch();
            engineNote.Pitch.Alter = note.Pitch.Alter;
            engineNote.Pitch.Octave = note.Pitch.Octave;
            engineNote.Pitch.Step = note.Pitch.Step;
            engineNote.Staff = note.Staff;
            engineNote.Type = note.Type;
            engineNote.Voice = note.Voice;
            engineNote.IsRest = note.IsRest;
            /**
             * Individual notes in a chord can be arpeggiated without the entire chord being arpeggiated.
             */
            if (note.Notations != null && note.Notations.IsArpeggiated)
            {
                engineNote.IsArpeggiated = note.Notations.IsArpeggiated;
            }
            return engineNote;
        }

        public int GetAdvancedClock(SortedDictionary<int, List<MeasureElement>> engineMeasureElements, MusicXml.Domain.MeasureElement nextMeasureElement, MusicXml.Domain.Note note, int currentClock)
        {
            if (note.IsChord ||
                (nextMeasureElement != null &&
                 nextMeasureElement.Type == MusicXml.Domain.MeasureElementType.Note &&
                ((MusicXml.Domain.Note)nextMeasureElement.Element).IsChord))
            {
                // If next note is chord, do not advance clock
                return currentClock;
            }
            else
            {
                return currentClock += note.Duration;
            }
        }
    }
}
