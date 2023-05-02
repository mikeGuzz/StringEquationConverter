using StringEquationConverter.FExceptions;
using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.NodeTypes.Operators.UnaryOperators
{
    public class Negative : UnaryOperator
    {
        public Negative() { }
        public Negative(FHValue left) : base(left) { }

        public override FFraction ToFraction()
        {
            if (LeftOperand is null)
                throw new FInvalidOperationException();
            return LeftOperand.ToFraction() * -1d;
        }

        public override char GetOperatorSing() => '-';
    }
}
