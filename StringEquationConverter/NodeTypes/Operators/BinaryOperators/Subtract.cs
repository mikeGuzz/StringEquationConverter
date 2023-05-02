using StringEquationConverter.FExceptions;
using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.NodeTypes.Operators.BinaryOperators
{
    public class Subtract : BinaryOperator
    {
        public Subtract() : base() { }

        public Subtract(FHValue left) : base(left) { }

        public Subtract(FHValue left, FHValue right) : base(left, right) { }

        public override FFraction ToFraction()
        {
            if (LeftOperand is null || RightOperand is null)
                throw new FInvalidOperationException($"{nameof(Subtract)}: operator operand missing.");
            return LeftOperand.ToFraction() - RightOperand.ToFraction();
        }

        public override int GetPriority() => 0;

        public override char GetOperatorSing() => '-';
    }
}
