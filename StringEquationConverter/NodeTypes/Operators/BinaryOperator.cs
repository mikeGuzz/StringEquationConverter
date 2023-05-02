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

            if (IsDeepEmpty() && operand is BinaryOperator biOperand)//встраивание оператора
            {
                // перехватываем
                if (biOperand.GetPriority() == GetPriority() || biOperand.GetPriority() < GetPriority())
                {
                    if (biOperand.RightOperand is not null)
                    {
                        RightOperand = biOperand.RightOperand;
                        biOperand.RightOperand = null;
                    }
                    if (biOperand.LeftOperand is not null)
                    {
                        LeftOperand = biOperand.LeftOperand;
                        biOperand.LeftOperand = null;
                    }
                    biOperand.LeftOperand = this;
                    return null;
                }
                else// встраиваем
                {
                    if (RightOperand is UnaryOperator op)
                    {
                        if (op.SAddOperand(operand) == null)
                            RightOperand = operand;
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
                //return false;
            {
                if (operand is UnaryOperator)
                {
                    res = ((UnaryOperator)operand).SAddOperand(RightOperand);
                    RightOperand = operand;
                    return res;
                }
                return false;
            }
            if (((UnaryOperator)RightOperand).SAddOperand(operand) is null)
            {
                RightOperand = operand;
            }
            return true;
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

        //public bool SAddOperatorAsOperand(BinaryOperator op)
        //{
        //    if (op.GetPriority() == GetPriority())
        //    {
        //        Move(op);
        //        op.SAddOperand(this);
        //        return true;
        //    }
        //    else
        //    {
        //        if(op.GetPriority() > GetPriority())
        //        {
        //            if (RightOperand is BinaryOperator biRight)
        //                biRight.SAddOperatorAsOperand(op);
        //            SAddOperand(op);
        //        }
        //        else
        //        {
        //            Move(op);
        //            op.SAddOperand(this);
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public abstract int GetPriority();

        public override string ToString()
        {
            var leftStr = LeftOperand is null ? "null" : LeftOperand.ToString();
            var rightStr = RightOperand is null ? "null" : RightOperand.ToString();
            return $"{leftStr} {GetOperatorSing()} {rightStr}";
        }
    }
}
