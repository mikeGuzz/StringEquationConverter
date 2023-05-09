using StringEquationConverter.FExceptions;
using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StringEquationConverter.NodeTypes.Operators
{
    public abstract class BinaryOperator : UnaryOperator
    {
        public IFHValue? RightOperand { get; set; }

        public BinaryOperator() : base() { }

        public BinaryOperator(IFHValue left) : base(left) { }

        public BinaryOperator(IFHValue left, IFHValue right) : base(left)
        {
            RightOperand = right;
        }

        public override FFraction? Simplify()
        {
            var tL = LeftOperand?.Simplify();
            var tR = RightOperand?.Simplify();
            if(tL is not null)
                LeftOperand = tL;
            if(tR is not null)
                RightOperand = tR;
            return (tL is not null && tR is not null) ? ToFraction() : null;
        }

        public override bool? SAddOperand(IFHValue operand)
        {
            var res = base.SAddOperand(operand);
            if (res != false)
                return res;

            if (operand is BinaryOperator biOperand && IsDeepEmpty())
            {
                //если операнд бинарный оператор, и его приориетность такая же или ниже,
                //тогда алгоритм помещает в левый операнд операнда
                //и меняет его местами (это происходит на стороне вызывающей функции).
                if (biOperand.GetPriority() == GetPriority() || biOperand.GetPriority() < GetPriority())
                {
                    biOperand.LeftOperand = this;
                    return null;
                }
                else//в ином случае - встраиваем
                {
                    if (RightOperand is UnaryOperator)
                    {
                        res = ((UnaryOperator)RightOperand).SAddOperand(operand);
                        if (res is null)
                            RightOperand = operand;
                        else if (res == false)
                            return res;
                    }
                    else
                        RightOperand = operand;
                }
                return true;
            }
            if(RightOperand is null)
            {
                RightOperand = operand;
                return true;
            }
            if (RightOperand is not UnaryOperator)
                return false;

            res = ((UnaryOperator)RightOperand).SAddOperand(operand);
            if (res is null)
            {
                RightOperand = operand;
                return true;
            }
            return res;
        }

        public override bool IsEmpty()
        {
            return base.IsEmpty() || RightOperand is null;
        }

        public override bool IsDeepEmpty()
        {
            var res = base.IsDeepEmpty();
            if (res)
                return true;
            if (RightOperand is UnaryOperator op)
                return op.IsDeepEmpty();
            return IsEmpty();
        }

        public abstract int GetPriority();

        public override string ToString()
        {
            var leftStr = LeftOperand is null ? "null" : LeftOperand.ToString();
            var rightStr = RightOperand is null ? "null" : RightOperand.ToString();
            return $"{leftStr} {GetOperatorSing()} {rightStr}";
        }
    }
}
