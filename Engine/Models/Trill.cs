using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    /// <summary>
    /// Players are free to control whether the above or below note is trilled by pressing either the note above or below the start note when playing the second note of the trill.
    /// </summary>
    public class Trill : MeasureElement
    {
        public Note StartNote { get; set; }
    }
}
