﻿using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter
{
    public interface IFHValue
    {
        public FFraction ToFraction();
        public FFraction? Simplify();
    }
}
