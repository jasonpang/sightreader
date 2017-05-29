using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Measure
    {
        public int MeasureElementIndex { get; set; }

        public List<MeasureElement> Elements { get; set; } = new List<MeasureElement>();

        public override string ToString()
        {
            return $"Measure ({Elements.Count} elements)";
        }
    }
}
