using Engine.Builder.MusicXml;
using Engine.Errors;
using Engine.Models;
using Engine.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Engine.Builder
{
    public class MeasureBuilder
    {
        public scorepartwisePartMeasure RawMeasure { get; set; }
        public SortedDictionary<int, IList<IMeasureElement>> MeasureElements { get; set; }
        public SortedDictionary<int, Measure> Measures { get; set; }
        public int Clock { get; set; }
        public HashSet<Type> ProcessOnly { get; set; }

        public MeasureBuilder(Stream musicXmlMeasure)
        {
            RawMeasure = DeserializeMusicXmlMeasure(musicXmlMeasure);
            Measures = new SortedDictionary<int, Measure>();
            MeasureElements = new SortedDictionary<int, IList<IMeasureElement>>();
            ProcessOnly = new HashSet<Type>();
        }

        public MeasureBuilder(scorepartwisePartMeasure rawMeasure, SortedDictionary<int, Measure> measures)
        {
            RawMeasure = rawMeasure;
            Measures = measures;
            MeasureElements = new SortedDictionary<int, IList<IMeasureElement>>();
            ProcessOnly = new HashSet<Type>();
        }

        public static scorepartwisePartMeasure DeserializeMusicXmlMeasure(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(scorepartwisePartMeasure), new XmlRootAttribute("measure"));
            try
            {
                scorepartwisePartMeasure measure = (scorepartwisePartMeasure)serializer.Deserialize(stream);
                return measure;
            }
            catch (Exception ex) when (ex.InnerException is XmlException)
            {
                throw new InvalidMusicXmlDocumentException(ex.InnerException as XmlException);
            }
        }

        public Measure BuildMeasure()
        {
            var measure = new Measure();
            ProcessRawMeasureElements();
            measure.Elements = MeasureElements.Values.Select(elements => new ElementGroup(elements)).ToList();
            return measure;
        }

        public void ProcessRawMeasureElements()
        {
            for (var RawElementIndex = 0; RawElementIndex < RawMeasure.Items.Length; RawElementIndex++)
            {
                var element = RawMeasure.Items[RawElementIndex];
                /*
                 * Some operations, like whether to advance the clock by a note's duration, require peeking ahead one element.
                 */
                object nextNoteElement = null;
                if (RawElementIndex + 1 < RawMeasure.Items.Length)
                {
                    nextNoteElement = RawMeasure.Items.Skip(RawElementIndex + 1).FirstOrDefault(rawMeasureItem => rawMeasureItem.GetType() == (typeof(note)));
                }

                if (ProcessOnly.Count > 0 && !ProcessOnly.Contains(element.GetType()))
                {
                    continue;
                }
                if (element.GetType() == typeof(backup))
                {
                    BuildMeasureElement_Backup((backup)element);
                }
                else if (element.GetType() == typeof(forward))
                {
                    BuildMeasureElement_Forward((forward)element);
                }
                else if (element.GetType() == typeof(note))
                {
                    BuildMeasureElement_Note((note)element, (note)nextNoteElement);
                }
            }
        }

        public void BuildMeasureElement_Backup(backup element)
        {
            if (!element.duration.IsInteger())
            {
                throw new InvalidMusicXmlDocumentException(null, $"Backup duration {element.duration.ToString()} should be an integer.");
            }
            Clock -= (int)element.duration;
        }

        public void BuildMeasureElement_Forward(forward element)
        {
            if (!element.duration.IsInteger())
            {
                throw new InvalidMusicXmlDocumentException(null, $"Forward duration {element.duration.ToString()} should be an integer.");
            }
            Clock += (int)element.duration;
        }

        public void BuildMeasureElement_Note(note element, note nextNoteElement)
        {
            var note = BuildMeasureElement_Note_Process(element);
            var nextNote = BuildMeasureElement_Note_Process(nextNoteElement);

            if (IsIdenticalNoteBacktrack(note))
            {
                // Move clock forward by duplicated not duration, but don't actually add the note
                Clock += note.Duration;
            }
            else
            {
                AddNote(Clock, note);
                Clock += BuildMeasureElement_Note_GetDurationToAdvance(note, nextNote);
            }
        }

        private bool IsIdenticalNoteBacktrack(Note note)
        {
            if (MeasureElements.Count == 0 || !MeasureElements.ContainsKey(Clock))
            {
                return false;
            }
            var identicalPitch = MeasureElements[Clock].FirstOrDefault(otherNote => ((Note)otherNote).Pitch == note.Pitch);
            return (identicalPitch != null);
        }

        private void AddNote(int clockTime, Note note)
        {
            if (!MeasureElements.ContainsKey(clockTime))
            {
                MeasureElements.Add(clockTime, new List<IMeasureElement>());
            }
            if (MeasureElements[clockTime] == null)
            {
                MeasureElements[clockTime] = new List<IMeasureElement>();
            }
            MeasureElements[clockTime].Add(note);
        }

        public Note BuildMeasureElement_Note_Process(note element)
        {
            if (element == null)
            {
                return null;
            }

            var note = new Note();
            if (element.staff != null)
            {
                note.Staff = Convert.ToInt32(element.staff);
            }
            if (element.voice != null)
            {
                note.Voice = Convert.ToInt32(element.voice);
            }
            if (element.type != null)
            {
                note.Type = element.type.Value.ToString().ToLower();
            }
            for (int elementIndex = 0; element.Items != null && elementIndex < element.Items.Length; elementIndex++)
            {
                var item = element.Items[elementIndex];
                var itemType = element.ItemsElementName[elementIndex];

                switch (itemType)
                {
                    case ItemsChoiceType1.chord:
                        BuildMeasureElement_Note_Chord(note, element);
                        break;
                    case ItemsChoiceType1.cue:
                        BuildMeasureElement_Note_Cue(note, element);
                        break;
                    case ItemsChoiceType1.duration:
                        BuildMeasureElement_Note_Duration(note, element, (decimal)item);
                        break;
                    case ItemsChoiceType1.grace:
                        BuildMeasureElement_Note_Grace(note, element, (grace)item);
                        break;
                    case ItemsChoiceType1.pitch:
                        BuildMeasureElement_Note_Pitch(note, element, (pitch)item);
                        break;
                    case ItemsChoiceType1.rest:
                        BuildMeasureElement_Note_Rest(note, element, (rest)item);
                        break;
                    case ItemsChoiceType1.tie:
                        BuildMeasureElement_Note_Tie(note, element, (tie)item);
                        break;
                    case ItemsChoiceType1.unpitched:
                        BuildMeasureElement_Note_Unpitched(note, element, (unpitched)item);
                        break;
                    default:
                        throw new UnsupportedMusicXmlException(itemType.ToString());
                }
            }
            for (int notationIndex = 0; element.notations != null && notationIndex < element.notations.Length; notationIndex++)
            {
                var untypedNotation = element.notations[notationIndex];
                for (int subNotationIndex = 0; untypedNotation.Items != null && subNotationIndex < untypedNotation.Items.Length; subNotationIndex++)
                {
                    var subNotation = untypedNotation.Items[subNotationIndex];
                    if (subNotation.GetType() == typeof(arpeggiate))
                    {
                        var arpeggiate = (arpeggiate)subNotation;
                        if (arpeggiate.directionSpecified)
                        {
                            if (arpeggiate.direction == updown.down)
                            {
                                note.IsArpeggiatedDown = true;
                            }
                            else if (arpeggiate.direction == updown.up)
                            {
                                note.IsArpeggiatedUp = true;
                            } else
                            {
                                throw new InvalidMusicXmlDocumentException(null, $"Invalid <arpeggiate> direction {arpeggiate.direction.ToString()} that is neither up nor down.");
                            }
                        } else
                        {
                            // Arpeggiate up by default
                            note.IsArpeggiatedUp = true;
                        }
                    }
                    else if (subNotation.GetType() == typeof(slur))
                    {
                        var slur = (slur)subNotation;
                        if (slur.type == startstopcontinue.start)
                        {
                            note.IsSlurStart = true;
                        }
                        if (slur.type == startstopcontinue.stop)
                        {
                            note.IsSlurStop = true;
                        }
                    }
                }
            }
            return note;
        }

        public int BuildMeasureElement_Note_GetDurationToAdvance(Note note, Note nextNote)
        {
            var shouldNotAdvanceClock = (
                (nextNote != null && nextNote.IsChordTone) ||
                note.IsGrace
                );
            if (shouldNotAdvanceClock)
            {
                return 0;
            } else
            {
                //var sameStaveChordNotes = MeasureElements[Clock].Where(otherNote => ((Note)otherNote).Staff == note.Staff);
                //var longestNoteInSameStaveGroup = sameStaveChordNotes.OrderByDescending(otherNote => ((Note)otherNote).Duration).First();
                //return ((Note)longestNoteInSameStaveGroup).Duration;
                return note.Duration;
            }
        }

        public void BuildMeasureElement_Note_Chord(Note note, note element)
        {
            note.IsChordTone = true;
        }

        public void BuildMeasureElement_Note_Cue(Note note, note element)
        {
            note.IsSilent = true;
        }

        public void BuildMeasureElement_Note_Duration(Note note, note element, decimal duration)
        {
            if (!duration.IsInteger())
            {
                throw new InvalidMusicXmlDocumentException(null, $"Note duration {duration.ToString()} should be an integer.");
            }
            note.Duration = (int)duration;
        }

        public void BuildMeasureElement_Note_Grace(Note note, note element, grace grace)
        {
            note.IsGrace = true;
        }

        public void BuildMeasureElement_Note_Pitch(Note note, note element, pitch pitch)
        {
            if (pitch.alterSpecified)
            {
                note.Pitch.Alter = (int)pitch.alter;
            }
            note.Pitch.Octave = Convert.ToInt32(pitch.octave);
            note.Pitch.Step = pitch.step.ToLetterString();
        }

        public void BuildMeasureElement_Note_Rest(Note note, note element, rest rest)
        {
            note.IsRest = true;
        }

        public void BuildMeasureElement_Note_Tie(Note note, note element, tie tie)
        {
            if (tie.type == startstop.start)
            {
                note.IsTiedStart = true;
            }
            if (tie.type == startstop.stop)
            {
                note.IsTiedStop = true;
            }
        }

        public void BuildMeasureElement_Note_Unpitched(Note note, note element, unpitched unpitched)
        {
            note.IsSilent = true;
        }
    }
}
