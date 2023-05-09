using StringEquationConverter.FExceptions;
using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.NodeTypes.Operators.BinaryOperators
{
    public class Multiply : BinaryOperator
    {
        public Multiply() : base() { }

        public Multiply(IFHValue left) : base(left) { }

        public Multiply(IFHValue left, IFHValue right) : base(left, right) { }

        public override FFraction ToFraction()
        {
            if (LeftOperand is null || RightOperand is null)
            {
                throw new FInvalidOperationException($"{GetType()}: operator operand missing.");
            }
            var temp = LeftOperand.ToFraction() * RightOperand.ToFraction();
            return temp;
        }

        public override int GetPriority() => 1;

        public override char GetOperatorSing() => '*';
    }
}
