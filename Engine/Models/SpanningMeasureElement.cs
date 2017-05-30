using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public abstract class SpanningMeasureElement : IMeasureElement
    {
        public int Duration { get; set; }
    }
}
