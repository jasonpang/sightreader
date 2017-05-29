using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Stave
    {
        public int MeasureIndex { get; set; }

        public List<Measure> Measures { get; set; } = new List<Measure>();
    }
}
