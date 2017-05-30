using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Engine.Errors
{
    public class InvalidMusicXmlDocumentException : SightReaderException
    {
        public InvalidMusicXmlDocumentException(XmlException innerException = null, String error = "There was an error parsing the XML document as MusicXML.")
            : base(error, innerException)
        {
        }
    }
}
