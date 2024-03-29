﻿using StringEquationConverter.ValueTypes;
using StringEquationConverter.FExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.NodeTypes.Operators.BinaryOperators
{
    public sealed class Add : BinaryOperator
    {
        public Add() : base() { }

        public Add(IFHValue left) : base(left) { }

        public Add(IFHValue left, IFHValue right) : base(left, right) { }

        public override FFraction ToFraction()
        {
            if (LeftOperand is null || RightOperand is null)
                throw new FInvalidOperationException($"{GetType()}: operator operand missing.");
            var temp = LeftOperand.ToFraction() + RightOperand.ToFraction();
            return temp;
        }

        public override int GetPriority() => 0;

        public override char GetOperatorSing() => '+';
    }
}
