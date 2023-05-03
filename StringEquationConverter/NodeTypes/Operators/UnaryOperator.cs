using StringEquationConverter.FExceptions;
using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.NodeTypes.Operators
{
    public abstract class UnaryOperator : TreeNode
    {
        public FHValue? LeftOperand { get; set; }

        public UnaryOperator() { }

        public UnaryOperator(FHValue? leftOperand) { LeftOperand = leftOperand; }

        public virtual bool IsEmpty()
        {
            return LeftOperand is null;
        }

        public virtual bool IsDeepEmpty()
        {
            if (LeftOperand is UnaryOperator op)
                return op.IsDeepEmpty();
            return IsEmpty();
        }

        public virtual bool? SAddOperand(FHValue operand)
        {
            if (LeftOperand is UnaryOperator unOp)
                return unOp.SAddOperand(operand);
            if (LeftOperand is null)
            {
                LeftOperand = operand;
                return true;
            }
            return false;
        }

        public abstract char GetOperatorSing();

        public override string ToString()
        {
            var leftStr = LeftOperand is null ? "null" : LeftOperand.ToString();
            return $"{GetOperatorSing()}({leftStr})";
        }
    }
}
