using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.FExceptions
{
    public abstract class FException : Exception
    {
        public FException() : base() { }
        public FException(string? message) : base(message) { }
        public FException(int i, string? message) : base($"(i={i}) {message}") { }
    }
}
