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
            //unOp.IsEmpty()
            if (LeftOperand is UnaryOperator unOp)
            {
                if(unOp.IsEmpty())
                    return unOp.SAddOperand(operand);
                //else if(operand is UnaryOperator unOperand)
                //{
                //    unOperand.SAddOperand(unOp);
                //    LeftOperand = operand;
                //    return true;
                //}
                else
                    return false;
            }
            if(LeftOperand is not null)
            {
                //if(operand is UnaryOperator)
                //{
                //    ((UnaryOperator)operand).SAddOperand(this);
                //    return null;
                //}
                return false;
            }
            LeftOperand = operand;
            return true;
        }

        public abstract char GetOperatorSing();

        public override string ToString()
        {
            var leftStr = LeftOperand is null ? "null" : LeftOperand.ToString();
            return $"{GetOperatorSing()}({leftStr})";
        }
    }
}
