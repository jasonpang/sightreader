using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Glissando : MeasureElement
    {
        public Note StartNote { get; set; }

        public Note StopNote { get; set; }
    }
}
