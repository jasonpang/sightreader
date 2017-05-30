using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class ScoreInfo
    {
        public String Work { get; set; } = "";
        public String Movement { get; set; } = "";
        public IDictionary<String, String> Creators { get; set; } = new Dictionary<String, String>();
        public IList<String> Credits { get; set; } = new List<String>();
        // TODO: Support Encoding
    }
}
