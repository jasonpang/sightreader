using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils.Extensions
{    
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Like String.Join(), but ignores null or empty strings.
        /// </summary>
        public static string JoinIgnoreNullOrEmpty<T>(this ICollection<T> collection)
        {
            return String.Join(" ", collection.Where(s => !String.IsNullOrEmpty(s?.ToString())));
        }
    }
}
