using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Playback
{
    public class PlayerVoiceMapping
    {
        public Pitch StartingPitch { get; set; }
        public Pitch EndingPitch { get; set; }
        public int Voice { get; set; }
    }
}
