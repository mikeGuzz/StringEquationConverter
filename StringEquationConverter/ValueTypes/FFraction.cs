using StringEquationConverter.FExceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEquationConverter.ValueTypes
{
    public struct FFraction : FHValue, IComparable
    {
        private double? den;

        public double Num { get; set; }
        public double Den
        {
            get => den is null ? 1d : (double)den;
            set 
            {
                if(value == 0d)
                    throw new FArgmentException("Divide by zero.");
                if(value < 0d)
                {
                    value *= -1d;
                    Num *= -1d;
                }
                den = value == 1d ? null : value; 
            }
        }

        public double D => Num / Den;

        public FFraction()
        {
            den = null;
            Num = 0;
        }

        public FFraction(double num, double? den)
        {
            Num = num;
            this.den = null;
            if(den is not null)
                Den = (double)den;
        }

        public FFraction ToFraction() => this;

        public static FFraction operator -(FFraction a) => new FFraction(-a.Num, a.Den);
        public static FFraction operator +(FFraction a) => a;

        public static FFraction operator +(FFraction a, FFraction b) => new FFraction(a.Num * b.Den + b.Num * a.Den, a.Den * b.Den);
        public static FFraction operator +(FFraction a, double b) => a + new FFraction(b, null);
        public static FFraction operator -(FFraction a, FFraction b) => a + (-b); 
        public static FFraction operator -(FFraction a, double b) => a + (-b);
        public static FFraction operator *(FFraction a, FFraction b) => new FFraction(a.Num * b.Num, a.Den * b.Den);
        public static FFraction operator *(FFraction a, double b) => a * new FFraction(b, null);
        public static FFraction operator /(FFraction a, FFraction b)
        {
            if (b.Num == 0d)
                throw new FArgmentException("Divide by zero.");
            return new FFraction(a.Num * b.Den, b.Num * a.Den);
        }
        public static FFraction operator /(FFraction a, double b) => a / new FFraction(b, null);

        public static bool operator ==(FFraction a, FFraction? b) => a.Equals(b);
        public static bool operator !=(FFraction a, FFraction? b) => !(a == b);
        public static bool operator >(FFraction a, FFraction? b) => a.CompareTo(b) > 0;
        public static bool operator <(FFraction a, FFraction? b) => !(a > b);
        public static bool operator >=(FFraction a, FFraction? b) => a > b || a == b;
        public static bool operator <=(FFraction a, FFraction? b) => a < b || a == b;

        public static bool operator true(FFraction a) => a.Num != 0d;
        public static bool operator false(FFraction a) => a.Num == 0d;

        public static FFraction operator ++(FFraction a) => new FFraction(a.Num++, a.Den);
        public static FFraction operator --(FFraction a) => new FFraction(a.Num--, a.Den);

        public FFraction Power(double power) => new FFraction(Math.Pow(Num, power), Math.Pow(Den, power));

        public FFraction Power(FHValue power)
        {
            //(a/b)^(x/y) = a^(x/y)/b^(x/y) = ((a^(1/y))^x)/((b^(1/y))^x)
            var y = power.ToFraction().D;
            return new FFraction(Math.Pow(Num, y), Math.Pow(Den, y));
        }

        public double GreatestCommonDivisor()
        {
            if (Num % 1 != 0 || Den % 1 != 0)
                return 1d;
            var a = Num;
            var b = Den;
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public double LeastCommonMultiple() => Num * Den / GreatestCommonDivisor();

        public FFraction Simplify()
        {
            var gcd = GreatestCommonDivisor();
            return new FFraction(Num / gcd, Den / gcd);
        }

        public override string ToString()
        {
            var sF = Simplify();
            return (sF.den is null || sF.Den == 1d) ? $"{sF.Num}" : $"({sF.Num}/{sF.Den})";
        }

        public override int GetHashCode()
        {
            return Num.GetHashCode() ^ Den.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (obj.GetType() != GetType())
                return false;
            return obj.GetHashCode() == GetHashCode();
        }

        public int CompareTo(object? obj)
        {
            if (ReferenceEquals(obj, null))
                return 1;
            if (obj.GetType() != GetType())
                throw new FInvalidOperationException($"{nameof(obj)} is not a {nameof(FFraction)}");
            var f = (FFraction)obj;
            return D.CompareTo(f.D);
        }
    }
}
