using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.NodeTypes
{
    public abstract class TreeNode : IFHValue
    {
        public abstract FFraction ToFraction();

        public abstract FFraction? Simplify();
    }
}
