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

        public Power(FHValue left) : base(left) { }

        public Power(FHValue left, FHValue right) : base(left, right) { }

        public override FFraction ToFraction()
        {
            if (LeftOperand is null || RightOperand is null)
                throw new FInvalidOperationException($"{nameof(Power)}: operator operand missing.");
            return LeftOperand.ToFraction().Power(RightOperand);
        }

        public override int GetPriority() => 2;

        public override char GetOperatorSing() => '^';
    }
}
