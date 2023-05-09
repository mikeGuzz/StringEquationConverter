using StringEquationConverter.FExceptions;
using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.NodeTypes.Operators.UnaryOperators
{
    public class Percentage : UnaryOperator
    {
        public Percentage() : base() { }
        public Percentage(IFHValue operand) : base(operand) { }

        public override FFraction ToFraction()
        {
            if (LeftOperand is null)
                throw new FInvalidOperationException($"{nameof(Percentage)}: operator operand missing.");
            return LeftOperand.ToFraction() / 100d;
        }

        public override char GetOperatorSing() => '%';
    }
}
