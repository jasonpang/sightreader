using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public interface ISpanningMeasureElement : IMeasureElement
    {
        int Duration { get; set; }
    }
}
