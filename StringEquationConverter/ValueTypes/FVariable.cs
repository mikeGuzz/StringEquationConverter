using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.ValueTypes
{
    public sealed class FVariable
    {
        public string Name { get; set; }
        public double Value { get; set; }

        public FVariable(string name, double value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString() => Name;
    }
}
