using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Mordent : MeasureElement
    {
        public bool IsUpper { get; set; }
        public Note StartNote { get; set; }
    }
}
