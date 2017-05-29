using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Measure : List<MeasureElement>
    {
        public int MeasureElementIndex { get; set; }
    }
}
