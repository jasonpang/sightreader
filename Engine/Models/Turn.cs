using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Turn : MeasureElement
    {
        public bool IsInverted { get; set; }
        public Note TargetNote { get; set; }
    }
}
