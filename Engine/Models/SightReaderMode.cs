using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    /// <summary>
    /// Describes the level of assistance the program provides to the player.
    /// </summary>
    public enum SightReaderMode
    {
        /// <summary>
        /// No assistance (like a normal piano). All notes and controller events are forwarded untouched.
        /// </summary>
        Passthrough,

        /// <summary>
        /// Main feature! Correct the notes played automatically, as well as other sorts of magic.
        /// </summary>
        Sightreading,
    }
}