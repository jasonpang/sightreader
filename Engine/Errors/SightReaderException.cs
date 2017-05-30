using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Errors
{
    public abstract class SightReaderException : Exception
    {
        public SightReaderException(String message, Exception innerException)
            : base(message, innerException) { }
    }
}
