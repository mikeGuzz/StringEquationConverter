using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.FExceptions
{
    public class FDivideByZeroException : FException
    {
        public FDivideByZeroException() : base() { }
        public FDivideByZeroException(string? message) : base(message) { }
        public FDivideByZeroException(int i, string? message) : base(i, message) { }
    }
}
