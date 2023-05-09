using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringEquationConverter.FExceptions;

namespace StringEquationConverter.ValueTypes
{
    public sealed class FVariable : IFHValue
    {
        public static readonly FVariable X = new FVariable("X", new FFraction());
        public static readonly List<FVariable> UserVars = new List<FVariable>()
        {
            new FVariable("pi", new FFraction(Math.PI, null)),
            new FVariable("e", new FFraction(Math.E, null)),
            new FVariable("tau", new FFraction(Math.Tau, null)),
        };

        public static bool TryParse(string name, out FVariable? value)
        {
            value = null;
            var coll = UserVars.Where(i => i.Name == name);
            if (coll.Count() != 1)
                return false;
            value = coll.Single();
            return true;
        }

        public static int IndexOf(string name)
        {
            for(int i = 0; i < UserVars.Count; i++)
            {
                if (UserVars[i].Name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        private FFraction value;

        public readonly bool IsConst;
        public string Name { get; set; }
        public FFraction Value
        {
            get => value;
            set
            {
                if (IsConst)
                    throw new FInvalidOperationException("Trying to change value of const variable.");
                this.value = value;
            }
        }

        public FVariable(string name, FFraction value, bool isConst = false)
        {
            Name = name;
            Value = value;
            IsConst = isConst;
        }

        public override string ToString() => Name;

        public FFraction ToFraction() => value;

        public FFraction? Simplify() => null;
    }
}
