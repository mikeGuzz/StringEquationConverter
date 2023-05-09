using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.NodeTypes
{
    public class FunctionNode : TreeNode
    {
        public FFunction FuncPtr { get; set; }
        public readonly List<IFHValue> Args;

        public FunctionNode(FFunction funcPtr, params IFHValue[] args)
        {
            FuncPtr = funcPtr;
            Args = new List<IFHValue>(args);
        }

        public override FFraction ToFraction()
        {
            return FuncPtr.CallLg(Args.Select(i => i.ToFraction()).ToArray());
        }

        public override FFraction? Simplify()
        {
            bool n = true;
            Args.ForEach(i =>
            {
                var temp = i.Simplify();
                if (temp is not null)
                    i = temp;
                else if (n)
                    n = false;
            });
            return n ? ToFraction() : null;
        }
    }
}
