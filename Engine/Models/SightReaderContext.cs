using Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class SightReaderContext
    {
        public InputDevice Input { get; private set; }

        public OutputDevice Output { get; private set; }

        public SightReaderMode Mode { get; private set; }
    }
}
