using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter
{
    public static class Extensions
    {
        public static bool IsBinaryOperator(this char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/' || c == '^'; 
        }

        public static bool IsUnaryOperator(this char c)
        {
            return c == '%';
        }

        public static bool IsOperator(this char c)
        {
            return c.IsBinaryOperator() || c.IsUnaryOperator();
        }

        public static bool IsBuildedValue(this StringBuilder str)
        {
            return str.Length > 0;
        }

        public static bool IsBuildedValue(this string str)
        {
            return str.Length > 0;
        }
    }
}
