using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter
{
    public static class FDefinitions
    {
        public static readonly FVariable X = new FVariable("X", 0d);
        public static readonly FVariable PI = new FVariable("PI", Math.PI);
        public static readonly FVariable E = new FVariable("E", Math.E);
        public static readonly FVariable Tau = new FVariable("Tau", Math.Tau);

        public static List<FVariable> UserVars = new List<FVariable>();
    }
}
