using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.FExceptions
{
    public class FInvalidOperationException : FException
    {
        public FInvalidOperationException() : base() { }
        public FInvalidOperationException(string? message) : base(message) { }
        public FInvalidOperationException(int i, string? message) : base(i, message) { }
    }
}
