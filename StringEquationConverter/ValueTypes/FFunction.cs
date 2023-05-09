using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringEquationConverter.FExceptions;

namespace StringEquationConverter.ValueTypes
{
    public delegate FFraction FFuncLgDelegate(params FFraction[] args);

    public sealed class FFunction
    {
        public static readonly List<FFunction> Funcs = new List<FFunction>()
        {
            new FFunction("pow", i => { CArgsN(i.Length); return i[0].Power(2d); }, "'a' raised to the power of 2."),
            new FFunction("pow", i => { CArgsN(i.Length, 2); return i[0].Power(i[1]); }, "'a' raised to the power of 'b'.", 2),
            new FFunction("sqrt", i => { CArgsN(i.Length); return i[0].Sqrt(); }, "Square root of 'a'."),
            new FFunction("cbrt", i => { CArgsN(i.Length); return i[0].Cbrt(); }, "Cube root of 'a'."),
            new FFunction("root", i => { CArgsN(i.Length, 2); return i[0].Power(i[1].Power(-1d)); }, "'a' is raised to the power of 'b', where 'b' is raised to the power of -1.", 2),
            new FFunction("abs", i => { CArgsN(i.Length); return i[0].Absolute(); }, "Geometric module 'a'."),
            new FFunction("exp", i => { CArgsN(i.Length); return new FFraction(Math.Exp(i[0].D), null); }, "E raised to the power of 'a'."),
            new FFunction("ln", i => { CArgsN(i.Length); return new FFraction(Math.Log(i[0].D), null); }),
            new FFunction("lg", i => { CArgsN(i.Length); return new FFraction(Math.Log10(i[0].D), null); }),
            new FFunction("log", i => { CArgsN(i.Length); return new FFraction(Math.Log10(i[0].D), null); }),
            new FFunction("log", i => { CArgsN(i.Length, 2); return new FFraction(Math.Log(i[0].D, i[1].D), null); }, null, 2),
            new FFunction("rand", i => { CArgsN(i.Length, 2); return new FFraction(Random.Shared.Next((int)i[0].D, (int)i[1].D), null); }, null, 2),
            //trigonometric functions:
            new FFunction("sin", i => { CArgsN(i.Length); return new FFraction(Math.Sin(i[0].D), null); }),
            new FFunction("cos", i => { CArgsN(i.Length); return new FFraction(Math.Cos(i[0].D), null); }),
            new FFunction("tan", i => { CArgsN(i.Length); return new FFraction(Math.Tan(i[0].D), null); }),
            new FFunction("sinh", i => { CArgsN(i.Length); return new FFraction(Math.Sinh(i[0].D), null); }),
            new FFunction("cosh", i => { CArgsN(i.Length); return new FFraction(Math.Cosh(i[0].D), null); }),
            new FFunction("tanh", i => { CArgsN(i.Length); return new FFraction(Math.Tanh(i[0].D), null); }),
            new FFunction("asin", i => { CArgsN(i.Length); return new FFraction(Math.Asin(i[0].D), null); }),
            new FFunction("acos", i => { CArgsN(i.Length); return new FFraction(Math.Acos(i[0].D), null); }),
            new FFunction("atan", i => { CArgsN(i.Length); return new FFraction(Math.Atan(i[0].D), null); }),
            new FFunction("asinh", i => { CArgsN(i.Length); return new FFraction(Math.Asinh(i[0].D), null); }),
            new FFunction("acosh", i => { CArgsN(i.Length); return new FFraction(Math.Acosh(i[0].D), null); }),
            new FFunction("atanh", i => { CArgsN(i.Length); return new FFraction(Math.Atanh(i[0].D), null); }),
            new FFunction("atan2", i => { CArgsN(i.Length, 2); return new FFraction(Math.Atan2(i[0].D, i[1].D), null); }, null, 2),
        };

        public static int IndexOf(string name, int argsCount)
        {
            for (int i = 0; i < Funcs.Count; i++)
            {
                if (Funcs[i].ArgsCount == argsCount && Funcs[i].Name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        public static bool Contains(string name)
        {
            return Funcs.Any(i => i.Name == name);
        }

        private static void CArgsN(int n1, int n2 = 1)
        {
            if (n1 != n2)
                throw new FArgmentException("Invalid count of arguments.");
        }

        public string Name { get; set; }
        public string? Desciption { get; set; }
        public readonly int ArgsCount;
        public FFuncLgDelegate Lg { get; set; }

        public FFunction(string name, FFuncLgDelegate lg)
        {
            Name = name;
            ArgsCount = 1;
            Lg = lg;
        }

        public FFunction(string name, FFuncLgDelegate lg, string? description, int argsCount = 1)
        {
            Name = name;
            Desciption = description;
            ArgsCount = argsCount;
            Lg = lg;
        }

        public FFraction CallLg(params FFraction[] args)
        {
            return Lg(args);
        }
    }
}
