using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Part
    {
        public List<Stave> Staves { get; set; } = new List<Stave>();

        public override string ToString()
        {
            return $"Part ({Staves.Count} staves)";
        }
    }
}
