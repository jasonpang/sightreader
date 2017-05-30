using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class ScorePart
    {
        public String Id { get; set; } = "";

        public IList<Measure> Measures { get; set; } = new List<Measure>();

        public override string ToString()
        {
            return $"ScorePart '{Id ?? "(untitled)"}' ({Measures.Count} measures)";
        }
    }
}
