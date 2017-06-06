using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Engine.Errors
{
    public class UnsupportedMusicXmlException : SightReaderException
    {
        public UnsupportedMusicXmlException(String elementTypeName)
            : base($"The MusicXML element <{elementTypeName}> is unsupported.", null)
        {
        }
    }
}
