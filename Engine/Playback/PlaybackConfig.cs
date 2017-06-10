using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Playback
{
    public class PlaybackConfig
    {
        public IList<PlayerVoiceMapping> PlayerVoiceMappings { get; set; } = new List<PlayerVoiceMapping>();

    }
}
