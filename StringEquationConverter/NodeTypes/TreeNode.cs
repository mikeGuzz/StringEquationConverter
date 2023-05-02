using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.NodeTypes
{
    public abstract class TreeNode : FHValue
    {
        public abstract FFraction ToFraction();
    }
}
