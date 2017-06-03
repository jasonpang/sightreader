using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils.Extensions
{
    public static class DecimalExtensions
    {
        public static bool IsInteger(this decimal d)
        {
            return Math.Floor(d) == Math.Ceiling(d);
        }
    }
}
