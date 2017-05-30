using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Measure
    {
        public IList<IMeasureElement> Elements { get; set; } = new List<IMeasureElement>();

        public override string ToString()
        {
            return $"Measure ({Elements.Count} elements)";
        }
    }
}
