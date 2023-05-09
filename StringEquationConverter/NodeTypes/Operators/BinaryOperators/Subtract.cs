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

        public Subtract(IFHValue left) : base(left) { }

        public Subtract(IFHValue left, IFHValue right) : base(left, right) { }

        public override FFraction ToFraction()
        {
            if (LeftOperand is null || RightOperand is null)
                throw new FInvalidOperationException($"{GetType()}: operator operand missing.");
            return LeftOperand.ToFraction() - RightOperand.ToFraction();
        }

        public override int GetPriority() => 0;

        public override char GetOperatorSing() => '-';
    }
}
