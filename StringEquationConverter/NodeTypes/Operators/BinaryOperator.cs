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
        public FHValue? RightOperand { get; set; }

        public BinaryOperator() : base() { }

        public BinaryOperator(FHValue left) : base(left) { }

        public BinaryOperator(FHValue left, FHValue right) : base(left)
        {
            RightOperand = right;
        }

        public override bool? SAddOperand(FHValue operand)
        {
            var res = base.SAddOperand(operand);
            if (res != false)
                return res;

            if (operand is BinaryOperator biOperand && IsDeepEmpty())//встраивание оператора
            {
                //если операнд бинарный оператор, и его приориетность такая же или ниже, чем наша,
                //тогда алгоритм помещает в левый операнд операнда (при условии что он пустой)
                //и меняет его местами (это происходит вне этого алгоритма, а если быть
                //точнее то на стороне вызывающей функции. Для того, чтобы указать что
                //мы хотим перемещение мы возвращаем null)
                //
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
