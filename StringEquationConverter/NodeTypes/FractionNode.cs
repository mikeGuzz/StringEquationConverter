using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.NodeTypes
{
    public class FractionNode : TreeNode
    {
        public FFraction Fraction { get; set; }

        public FractionNode() { }

        public FractionNode(FFraction fraction)
        {
            Fraction = fraction;
        }

        public override FFraction ToFraction()
        {
            return Fraction;
        }

        public override string ToString()
        {
            return Fraction.ToString();
        }
    }
}
