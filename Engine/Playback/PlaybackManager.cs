using Engine.Models;
using Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Engine.Playback
{
    public class PlaybackManager
    {
        public Score Score { get; set; }
        public IDictionary<int, int> VoiceMeasureMap { get; set; } = new Dictionary<int, int>();
        public IDictionary<int, int> VoiceMeasureElementMap { get; set; } = new Dictionary<int, int>();
        public SightReader SightReader { get; set; }
        private Dictionary<Midi.Pitch, List<Midi.Pitch>> OnOffMap { get; set; } = new Dictionary<Midi.Pitch, List<Midi.Pitch>>();
        public event EventHandler NoteProcessed;

        public PlaybackManager(SightReader sightReader, Score score)
        {
            SightReader = sightReader;
            Score = score;
            Reset();
        }

        public void Reset()
        {
            VoiceMeasureMap.Add(0, 0);
            VoiceMeasureMap.Add(1, 0);
            VoiceMeasureElementMap.Add(0, 0);
            VoiceMeasureElementMap.Add(1, 0);
        }

        public void OnNoteOn(NoteOnMessage note)
        {
            if (SightReader.Mode == SightReaderMode.Passthrough)
            {
                Debug.WriteLine($"({SightReader.Mode.ToString()}) Note On: {note.ToString()}");
                SightReader.Output.SendNoteOn(note.Channel, note.Pitch, note.Velocity);
            }
            else if (SightReader.Mode == SightReaderMode.Sightreading)
            {
                SightReadOn(note);
            }
        }

        public int GetVoice(Midi.Pitch pitch)
        {
            return (pitch >= Midi.Pitch.F4) ? 1 : 0;
        }

        private void SightReadOn(NoteOnMessage message)
        {
            var newPitches = GetCorrectedPitch(message.Pitch);

            var voice = GetVoice(message.Pitch);

            var notesOffList = new List<Midi.Pitch>();
            Debug.WriteLine($"Measure {GetMeasureIndex(voice) + 1}, Element {GetGroupElementIndex(voice) + 1}: {String.Join(" ", newPitches)}");
            foreach (var newPitch in newPitches)
            {
                try
                {
                    var replacementDictionary = new Dictionary<Midi.Pitch, List<Midi.Pitch>>();
                    foreach (KeyValuePair<Midi.Pitch, List<Midi.Pitch>> entry in OnOffMap)
                    {
                        if (entry.Value.Contains(newPitch))
                        {
                            replacementDictionary.Add(entry.Key, entry.Value.Where(pitch => pitch != newPitch).ToList());
                        }
                    }
                    foreach (KeyValuePair<Midi.Pitch, List<Midi.Pitch>> entry in replacementDictionary)
                    {
                        OnOffMap[entry.Key] = entry.Value;
                    }
                }
                catch (Exception ex) { }
                SightReader.Output.SendNoteOn(message.Channel, newPitch, message.Velocity);
                notesOffList.Add(newPitch);
            }
            OnOffMap[message.Pitch] = notesOffList;
        }

        private void SightReadOff(NoteOffMessage message)
        {
            if (OnOffMap.ContainsKey(message.Pitch))
            {
                var pitchesToRelease = OnOffMap[message.Pitch];
                foreach (var pitchToRelease in pitchesToRelease)
                {
                    SightReader.Output.SendNoteOff(message.Channel, pitchToRelease, message.Velocity);
                }
            }
        }

        public void OnNoteOff(NoteOffMessage note)
        {
            if (SightReader.Mode == SightReaderMode.Passthrough)
            {
                Debug.WriteLine($"({SightReader.Mode.ToString()}) Note Off: {note.ToString()}");
                SightReader.Output.SendNoteOff(note.Channel, note.Pitch, note.Velocity);
            }
            else if (SightReader.Mode == SightReaderMode.Sightreading)
            {
                SightReadOff(note);
                NoteProcessed?.Invoke(this, null);
            }
        }


        public void OnControlChange(ControlChangeMessage control)
        {
            Debug.WriteLine($"({SightReader.Mode.ToString()}) Control Change: {control.ToString()}");
            SightReader.Output.SendControlChange(control.Channel, control.Control, control.Value);
        }

        public int GetMeasureIndex(int voice)
        {
            return VoiceMeasureMap[voice];
        }

        public void SetMeasureIndex(int voice, int newMeasureIndex)
        {
            VoiceMeasureMap[voice] = newMeasureIndex;
        }

        public void IncrementMeasureIndex(int voice)
        {
            var measureIndex = GetMeasureIndex(voice);
            SetMeasureIndex(voice, measureIndex + 1);
        }

        public int GetGroupElementIndex(int voice)
        {
            return VoiceMeasureElementMap[voice];
        }

        public void SetGroupElementIndex(int voice, int newElementIndex)
        {
            VoiceMeasureElementMap[voice] = newElementIndex;
        }

        public void IncrementElementIndex(int voice)
        {
            var elementIndex = GetGroupElementIndex(voice);
            SetGroupElementIndex(voice, elementIndex + 1);
        }

        public Measure GetMeasure(int voice)
        {
            return Score.Parts[0].Measures[GetMeasureIndex(voice)];
        }

        public Measure GetPreviousMeasure(int voice)
        {
            return Score.Parts[0].Measures[GetMeasureIndex(voice) - 1];
        }

        public List<Measure> GetMeasures()
        {
            return Score.Parts[0].Measures.ToList();
        }

        public List<Models.Note> GetNotes(int voice)
        {
            var notes = GetMeasure(voice).Elements[GetGroupElementIndex(voice)].GroupElements.Cast<Models.Note>().ToList();
            return notes;
        }

        public void IncrementIndex(int voice)
        {
            // Otherwise, keep moving right to find an element group
            IncrementElementIndex(voice);
            // Even if it means going to the next measure
            if (GetGroupElementIndex(voice) >= GetMeasure(voice).Elements.Count)
            {
                IncrementMeasureIndex(voice);
                // Reset the element index for our new measure
                SetGroupElementIndex(voice, 0);
            }
        }

        public int GetStaff(int voice)
        {
            return voice == 0 ? 2 : 1;
        }

        public Models.ElementGroup GetPreviousPlayableGroup(int voice, int staff)
        {
            if (GetGroupElementIndex(voice) > 0 && 
                GetGroupElementIndex(voice) < GetMeasure(voice).Elements.Count)
            {
                return GetMeasure(voice).
                       Elements.
                       AsEnumerable().
                       Reverse().
                       Skip(GetMeasure(voice).Elements.Count - GetGroupElementIndex(voice)).
                       FirstOrDefault(
                            group => group.GroupElements.Where( 
                                n => ((Models.Note)n).IsPlayable(staff)
                            ).Count() > 0
                       );
            }
            else if (GetGroupElementIndex(voice) == 0 &&
                     GetMeasureIndex(voice) > 0)
            {
                // Find previous measure with a note (we might have a couple empty rest measures
                // But nvmd since its rare
                return GetPreviousMeasure(voice).
                       Elements.
                       AsEnumerable().
                       Reverse().
                       FirstOrDefault(
                           group => group.GroupElements.Where(
                               n => ((Models.Note)n).IsPlayable(staff)
                           ).Count() > 0
                       );
                // We're on the measure's first note, look at the last note of the previous measure for slurs that should be ties
            } else
            {
                throw new NotImplementedException();
            }
        }

        public bool IsNoteSlurThatShouldBeTied(Models.Note note, int voice, int staff)
        {
            if (note.IsSlurStop)
            {
                var previousPlayableGroup = GetPreviousPlayableGroup(voice, staff);
                foreach (var previousElement in previousPlayableGroup.GroupElements)
                {
                    var previousNote = (Models.Note)previousElement;
                    if (previousNote.IsSlurStart &&
                        previousNote.Pitch == note.Pitch &&
                        previousNote.Staff == note.Staff)
                    {
                        return true;
                    }
                }
                return false;
            }
            else return false;
        }

        public IList<Midi.Pitch> GetCorrectedPitch(Midi.Pitch inputPitch)
        {
            var correctedPitches = new List<Midi.Pitch>();

            var voice = GetVoice(inputPitch);
            var staff = GetStaff(voice);
            
            do
            {
                var elements1 = GetNotes(voice);
                if (GetNotes(voice).Where(note => note.IsPlayable(staff) && 
                !IsNoteSlurThatShouldBeTied(note, voice, staff)).Count() > 0)
                {
                    // If we've found an ElementGroup with at least one note on our staff, let's play it
                    break;
                }
                else
                {
                    // Otherwise, keep moving right to find an element group
                    // Even if it means going to the next measure
                    IncrementIndex(voice);
                    // If we're all out of measures, we're finished playing!
                    if (GetMeasureIndex(voice) >= GetMeasures().Count)
                    {
                        // Return empty notes
                        return new List<Midi.Pitch>();
                    }
                }
            } while (true);

            var elements2 = GetNotes(voice);
            // We should now be on an ElementGroup with at least one note on our staff
            foreach (var note in GetNotes(voice))
            {
                if (!note.IsPlayable(staff) ||
                    IsNoteSlurThatShouldBeTied(note, voice, staff))
                {
                    continue;
                }
                var midiNote = new Midi.Note(note.Pitch.Step[0], note.Pitch.Alter);
                correctedPitches.Add(midiNote.PitchInOctave(note.Pitch.Octave));
            }

            // Advance our element index
            IncrementIndex(voice);

            return correctedPitches;
        }
    }
}
