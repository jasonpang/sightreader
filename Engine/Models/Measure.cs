using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Measure
    {
        public IList<ElementGroup> Elements { get; set; } = new List<ElementGroup>();

        public override string ToString()
        {
            return $"Measure ({Elements.Count} elements)";
        }
    }
}
