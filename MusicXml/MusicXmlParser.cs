using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using MusicXml.Domain;
using Encoding = MusicXml.Domain.Encoding;
using System.Collections.Generic;

namespace MusicXml
{
	public static class MusicXmlParser
	{
		public static Score GetScore(string filename)
		{
			var document = GetXmlDocument(filename);

			var score = new Score();

			var movementTitleNode = document.SelectSingleNode("score-partwise/movement-title");
			if (movementTitleNode != null)
				score.MovementTitle = movementTitleNode.InnerText;

			score.Identification = GetIdentification(document);
			
			var partNodes = document.SelectNodes("score-partwise/part-list/score-part");
			
			if (partNodes != null)
			{
				foreach (XmlNode partNode in partNodes)
				{
					var part = new Part();
					score.Parts.Add(part);

					if (partNode.Attributes != null)
						part.Id = partNode.Attributes["id"].InnerText;
					
					var partNameNode = partNode.SelectSingleNode("part-name");
					
					if (partNameNode != null)
						part.Name = partNameNode.InnerText;

					var measuresXpath = string.Format("//part[@id='{0}']/measure", part.Id);

					var measureNodes = partNode.SelectNodes(measuresXpath);

					if (measureNodes != null)
					{
						foreach (XmlNode measureNode in measureNodes)
						{
							var measure = new Measure();

							if (measureNode.Attributes != null)
							{
								var measureWidthAttribute = measureNode.Attributes["width"];
								decimal w;
								if (measureWidthAttribute != null && decimal.TryParse(measureWidthAttribute.InnerText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,out w))
									measure.Width = w;
							}

							var attributesNode = measureNode.SelectSingleNode("attributes");

							if (attributesNode != null)
							{
								measure.Attributes = new MeasureAttributes();

								var divisionsNode = attributesNode.SelectSingleNode("divisions");
								if (divisionsNode != null)
									measure.Attributes.Divisions = Convert.ToInt32(divisionsNode.InnerText);

								var keyNode = attributesNode.SelectSingleNode("key");

								if (keyNode != null)
								{
									measure.Attributes.Key = new Key();

									var fifthsNode = keyNode.SelectSingleNode("fifths");
									if (fifthsNode != null)
										measure.Attributes.Key.Fifths = Convert.ToInt32(fifthsNode.InnerText);

									var modeNode = keyNode.SelectSingleNode("mode");
									if (modeNode != null)
										measure.Attributes.Key.Mode = modeNode.InnerText;
								}
								
								measure.Attributes.Time = GetTime(attributesNode);

								measure.Attributes.Clef = GetClef(attributesNode);
							}

							var childNodes = measureNode.ChildNodes;

							foreach (XmlNode node in childNodes)
							{
								MeasureElement measureElement = null;

								if (node.Name == "note")
								{
									var newNote = GetNote(node);
									measureElement = new MeasureElement {Type = MeasureElementType.Note, Element = newNote};
								}
								else if (node.Name == "backup")
								{
									measureElement = new MeasureElement {Type = MeasureElementType.Backup, Element = GetBackupElement(node)};
								}
								else if (node.Name == "forward")
								{
									measureElement = new MeasureElement {Type = MeasureElementType.Forward, Element = GetForwardElement(node)};
								}

								if (measureElement != null)
									measure.MeasureElements.Add(measureElement);
							}
							
							part.Measures.Add(measure);
						}
					}
				}
			}

			return score;
		}

		private static Forward GetForwardElement(XmlNode node)
		{
			var forward = new Forward();

			var forwardNode = node.SelectSingleNode("duration");

			if (forwardNode != null)
			{
				forward.Duration = Convert.ToInt32(forwardNode.InnerText);
			}

			return forward;
		}

		private static Backup GetBackupElement(XmlNode node)
		{
			var backup = new Backup();

			var backupNode = node.SelectSingleNode("duration");

			if (backupNode != null)
			{
				backup.Duration = Convert.ToInt32(backupNode.InnerText);
			}

			return backup;
		}

        private static Note GetNote(XmlNode noteNode)
        {
            var note = new Note();

            var typeNode = noteNode.SelectSingleNode("type");
            if (typeNode != null)
                note.Type = typeNode.InnerText;

            var voiceNode = noteNode.SelectSingleNode("voice");
            if (voiceNode != null)
                note.Voice = Convert.ToInt32(voiceNode.InnerText);

            var durationNode = noteNode.SelectSingleNode("duration");
            if (durationNode != null)
                note.Duration = Convert.ToInt32(durationNode.InnerText);

            note.Pitch = GetPitch(noteNode);

            var staffNode = noteNode.SelectSingleNode("staff");
            if (staffNode != null)
                note.Staff = Convert.ToInt32(staffNode.InnerText);

            var chordNode = noteNode.SelectSingleNode("chord");
            if (chordNode != null)
                note.IsChord = true;

            var restNode = noteNode.SelectSingleNode("rest");
            if (restNode != null)
                note.IsRest = true;

            var notationsNode = noteNode.SelectSingleNode("notations");
            if (notationsNode != null)
            {
                note.Notations = GetNotations(notationsNode);
            }

            var tieNodes = noteNode.SelectNodes("tie");
            foreach (XmlNode tieNode in tieNodes)
            {
                if (tieNode != null)
                {
                    if (note.Tie == null)
                    {
                        note.Tie = new Tie();
                    }
                    if (tieNode.Attributes != null)
                    {
                        if (tieNode.Attributes["type"].InnerText == "start")
                        {
                            note.Tie.IsStart = true;
                        }
                        if (tieNode.Attributes["type"].InnerText == "stop")
                        {
                            note.Tie.IsStop = true;
                        }
                    }
                }
            }

            var graceNode = noteNode.SelectSingleNode("grace");
            if (graceNode != null)
                note.IsGrace = true;

            var dotNode = noteNode.SelectSingleNode("dot");
            if (dotNode != null)
                note.IsDotted = true;

            var accidentalNode = noteNode.SelectSingleNode("accidental");
            if (accidentalNode != null)
                note.Accidental = accidentalNode.InnerText;

            return note;
        }

        private static Notations GetNotations(XmlNode noteNode)
        {
            var notations = new Notations();

            var notationsChildren = noteNode.ChildNodes;
            foreach (XmlNode node in notationsChildren)
            {
                if (node.Name == "arpeggiate")
                {
                    notations.IsArpeggiated = true;
                }
                if (node.Name == "glissando")
                {
                    notations.Glissando = GetGlissando(node);
                }
                if (node.Name == "ornaments")
                {
                    notations.Ornaments = GetOrnaments(node);
                }
                if (node.Name == "tied")
                {
                    if (node.Attributes != null)
                    {
                        notations.Tied = new Tied();
                        if (node.Attributes["type"].InnerText == "start")
                        {
                            notations.Tied.Type = StartStopContinueType.Start;
                        }
                        else if (node.Attributes["type"].InnerText == "stop")
                        {
                            notations.Tied.Type = StartStopContinueType.Stop;
                        }
                        else
                        {
                            notations.Tied.Type = StartStopContinueType.Continue;
                        }
                    }
                }
                if (node.Name == "tuplet")
                {
                    notations.IsTupliet = true;
                }
            }

            return notations;
        }

        private static Ornaments GetOrnaments(XmlNode noteNode)
        {
            var ornaments = new Ornaments();

            var ornamentsChildren = noteNode.ChildNodes;
            foreach (XmlNode node in ornamentsChildren)
            {
                if (node.Name == "delayed-inverted-turn")
                {
                    ornaments.IsTurn = true;
                }
                if (node.Name == "delayed-turn")
                {
                    ornaments.IsTurn = true;
                }
                if (node.Name == "inverted-turn")
                {
                    ornaments.IsTurn = true;
                }
                if (node.Name == "turn")
                {
                    ornaments.IsTurn = true;
                }
                if (node.Name == "trill")
                {
                    ornaments.IsTrill = true;
                }
                if (node.Name == "wavy-line")
                {
                    if (node.Attributes["type"].InnerText == "start" ||
                        node.Attributes["type"].InnerText == "continue")
                    {
                        ornaments.IsTrillStart = true;
                    } else
                    {
                        ornaments.IsTrillStop = true;
                    }
                }
            }

            return ornaments;
        }

        private static HorizontalTurn GetHorizontalTurn(XmlNode noteNode)
        {
            var notation = new HorizontalTurn();
            if (noteNode.Attributes != null)
            {
                if (noteNode.Attributes["start-note"] != null)
                {
                    notation.StartNote = GetStartNote(noteNode);
                }
                if (noteNode.Attributes["trill-step"] != null)
                {
                    notation.TrillStep = GetTrillStep(noteNode);
                }
                if (noteNode.Attributes["two-note-turn"] != null)
                {
                    notation.TwoNoteTurn = GetTwoNoteTurn(noteNode);
                }
            }

            return notation;
        }

        private static ArpeggiateNotation GetArpeggiate(XmlNode noteNode)
        {
            var notation = new ArpeggiateNotation();
            if (noteNode.Attributes != null)
            {
                if (noteNode.Attributes["direction"] != null)
                {
                    notation.Direction = GetUpDownDirection(noteNode);
                }
            }

            return notation;
        }

        private static GlissandoNotation GetGlissando(XmlNode noteNode)
        {
            var notation = new GlissandoNotation();
            if (noteNode.Attributes != null)
            {
                if (noteNode.Attributes["type"] != null)
                {
                    notation.Type = GetStartStop(noteNode);
                }
            }

            return notation;
        }

        private static UpDownDirection GetUpDownDirection(XmlNode noteNode)
        {
            if (noteNode.Attributes["direction"].InnerText == "down")
            {
                return UpDownDirection.Down;
            }
            else
            {
                return UpDownDirection.Up;
            }
        }

        private static StartNoteType GetStartNote(XmlNode noteNode)
        {
            if (noteNode.Attributes["start-note"].InnerText == "upper")
            {
                return StartNoteType.Upper;
            }
            else if (noteNode.Attributes["start-note"].InnerText == "main")
            {
                return StartNoteType.Main;
            }
            else
            {
                return StartNoteType.Below;
            }
        }

        private static TrillStepType GetTrillStep(XmlNode noteNode)
        {
            if (noteNode.Attributes["trill-step"].InnerText == "whole")
            {
                return TrillStepType.Whole;
            }
            else if (noteNode.Attributes["trill-step"].InnerText == "half")
            {
                return TrillStepType.Half;
            }
            else
            {
                return TrillStepType.Unison;
            }
        }

        private static TwoNoteTurnType GetTwoNoteTurn(XmlNode noteNode)
        {
            if (noteNode.Attributes["two-note-turn"].InnerText == "whole")
            {
                return TwoNoteTurnType.Whole;
            }
            else if (noteNode.Attributes["two-note-turn"].InnerText == "half")
            {
                return TwoNoteTurnType.Half;
            }
            else
            {
                return TwoNoteTurnType.None;
            }
        }

        private static StartStopType GetStartStop(XmlNode noteNode)
        {
            if (noteNode.Attributes["start-stop"].InnerText == "start")
            {
                return StartStopType.Start;
            }
            else
            {
                return StartStopType.Stop;
            }
        }

        private static StartStopContinueType GetStartStopContinue(XmlNode noteNode)
        {
            if (noteNode.Attributes["start-stop-continue"].InnerText == "start")
            {
                return StartStopContinueType.Start;
            }
            else if (noteNode.Attributes["start-stop-continue"].InnerText == "stop")
            {
                return StartStopContinueType.Stop;
            }
            else
            {
                return StartStopContinueType.Continue;
            }
        }

        private static Pitch GetPitch(XmlNode noteNode)
		{
			var pitch = new Pitch();
			var pitchNode = noteNode.SelectSingleNode("pitch");
			if (pitchNode != null)
			{
				var stepNode = pitchNode.SelectSingleNode("step");
				if (stepNode != null)
					pitch.Step = stepNode.InnerText[0];

				var alterNode = pitchNode.SelectSingleNode("alter");
				if (alterNode != null)
					pitch.Alter = Convert.ToInt32(alterNode.InnerText);

				var octaveNode = pitchNode.SelectSingleNode("octave");
				if (octaveNode != null)
					pitch.Octave = Convert.ToInt32(octaveNode.InnerText);
			}
			else
			{
				return null;
			}

			return pitch;
		}

		private static Clef GetClef(XmlNode attributesNode)
		{
			var clef = new Clef();

			var clefNode = attributesNode.SelectSingleNode("clef");

			if (clefNode != null)
			{
				var lineNode = clefNode.SelectSingleNode("line");
				if (lineNode != null)
					clef.Line = Convert.ToInt32(lineNode.InnerText);

				var signNode = clefNode.SelectSingleNode("sign");
				if (signNode != null)
					clef.Sign = signNode.InnerText;
			}
			return clef;
		}

		private static Time GetTime(XmlNode attributesNode)
		{
			var time = new Time();

			var timeNode = attributesNode.SelectSingleNode("time");
			if (timeNode != null)
			{
				var beatsNode = timeNode.SelectSingleNode("beats");

				if (beatsNode != null)
					time.Beats = Convert.ToInt32(beatsNode.InnerText);

				var beatTypeNode = timeNode.SelectSingleNode("beat-type");

				if (beatTypeNode != null)
					time.Mode = beatTypeNode.InnerText;

				var symbol = TimeSymbol.Normal;

				if (timeNode.Attributes != null)
				{
					var symbolAttribute = timeNode.Attributes["symbol"];

					if (symbolAttribute != null)
					{
						switch (symbolAttribute.InnerText)
						{
							case "common":
								symbol = TimeSymbol.Common;
								break;
							case "cut":
								symbol = TimeSymbol.Cut;
								break;
							case "single-number":
								symbol = TimeSymbol.SingleNumber;
								break;
						}
					}
				}

				time.Symbol = symbol;
			}
			return time;
		}

		private static Identification GetIdentification(XmlNode document)
		{
			var identificationNode = document.SelectSingleNode("score-partwise/identification");

			if (identificationNode != null)
			{
				var identification = new Identification();

				var composerNode = identificationNode.SelectSingleNode("creator[@type='composer']");
				identification.Composer = composerNode != null ? composerNode.InnerText : string.Empty;

				var rightsNode = identificationNode.SelectSingleNode("rights");
				identification.Rights = rightsNode != null ? rightsNode.InnerText : string.Empty;

				identification.Encoding = GetEncoding(identificationNode);

				return identification;
			}

			return null;
		}

		private static Encoding GetEncoding(XmlNode identificationNode)
		{
			var encodingNode = identificationNode.SelectSingleNode("encoding");

			var encoding = new Encoding();

			if (encodingNode != null)
			{
				encoding.Software = GetInnerTextOfChildTag(encodingNode, "software");

				encoding.Description = GetInnerTextOfChildTag(encodingNode, "encoding-description");

				var encodingDate = encodingNode.SelectSingleNode("encoding-date");
				if (encodingDate != null)
					encoding.EncodingDate = Convert.ToDateTime(encodingDate.InnerText);
			}

			return encoding;
		}

		private static string GetInnerTextOfChildTag(XmlNode encodingNode, string tagName)
		{
			var softwareStringBuilder = new StringBuilder();
			
			var encodingSoftwareNodes = encodingNode.SelectNodes(tagName);

			if (encodingSoftwareNodes != null)
			{
				foreach (XmlNode node in encodingSoftwareNodes)
				{
					softwareStringBuilder.AppendLine(node.InnerText);
				}
			}

			return softwareStringBuilder.ToString();
		}

		private static XmlDocument GetXmlDocument(string filename)
		{
			var document = new XmlDocument();

			var xml = GetFileContents(filename);
			document.XmlResolver = null;
			document.LoadXml(xml);

			return document;
		}

		private static string GetFileContents(string filename)
		{
			using (var fileStream = new FileStream(filename, FileMode.Open))
			using (var streamReader = new StreamReader(fileStream))
			{
				return streamReader.ReadToEnd();
			}
		}
	}
}
