using StringEquationConverter.FExceptions;
using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.NodeTypes.Operators.BinaryOperators
{
    public class Power : BinaryOperator
    {
        public Power() : base() { }

        public Power(IFHValue left) : base(left) { }

        public Power(IFHValue left, IFHValue right) : base(left, right) { }

        public override FFraction ToFraction()
        {
            if (LeftOperand is null || RightOperand is null)
                throw new FInvalidOperationException($"{GetType()}: operator operand missing.");
            return LeftOperand.ToFraction().Power(RightOperand);
        }

        public override int GetPriority() => 2;

        public override char GetOperatorSing() => '^';
    }
}
