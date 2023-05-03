using StringEquationConverter.FExceptions;
using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.NodeTypes.Operators.BinaryOperators
{
    public class Divide : BinaryOperator
    {
        public Divide() : base() { }

        public Divide(FHValue left) : base(left) { }

        public Divide(FHValue left, FHValue right) : base(left, right) { }

        public override FFraction ToFraction()
        {
            if (LeftOperand is null || RightOperand is null)
                throw new FInvalidOperationException($"{GetType()}: operator operand missing.");
            return LeftOperand.ToFraction() / RightOperand.ToFraction();
        }

        public override int GetPriority() => 1;

        public override char GetOperatorSing() => '/';
    }
}
