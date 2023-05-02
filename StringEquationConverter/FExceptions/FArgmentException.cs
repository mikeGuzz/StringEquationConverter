using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.FExceptions
{
    public class FArgmentException : FException
    {
        public FArgmentException() : base() { }
        public FArgmentException(string? message) : base(message) { }
        public FArgmentException(int i, string? message) : base(i, message) { }
    }
}
