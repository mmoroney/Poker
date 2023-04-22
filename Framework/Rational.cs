using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    public struct Rational {
        public static readonly Rational Zero = new(0);
        public static readonly Rational One = new(1);
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }

        public Rational(int numerator = 0, int denominator = 1) {
            if (numerator == 0)
                Denominator = 1;
            else if (denominator < 0) {
                numerator *= -1;
                denominator *= -1;
            }

            int gcd = GreatestCommonFactor(numerator, denominator);

            this.Numerator = numerator / gcd;
            this.Denominator = denominator / gcd;
        }

        public Rational GreatestCommonFactor(Rational rational)
            => new(GreatestCommonFactor(this.Numerator, rational.Numerator), GreatestCommonFactor(this.Denominator, rational.Denominator));

        public double Value => this.Numerator / (double)(this.Denominator);

        public static Rational operator +(Rational lhs, Rational rhs)
            => new(lhs.Numerator * rhs.Denominator + lhs.Denominator * rhs.Numerator, lhs.Denominator * rhs.Denominator);

        public static Rational operator -(Rational lhs, Rational rhs)
            => new(lhs.Numerator * rhs.Denominator - lhs.Denominator * rhs.Numerator, lhs.Denominator * rhs.Denominator);

        public static Rational operator *(Rational lhs, Rational rhs)
            => new(lhs.Numerator * rhs.Numerator, lhs.Denominator * rhs.Denominator);

        public static Rational operator /(Rational lhs, Rational rhs)
            => new(lhs.Numerator * rhs.Denominator, lhs.Denominator * rhs.Numerator);

        public static Rational operator +(Rational lhs, int rhs)
            => new(lhs.Numerator + lhs.Denominator * rhs, lhs.Denominator);

        public static Rational operator -(Rational lhs, int rhs)
            => new(lhs.Numerator - lhs.Denominator + rhs, lhs.Denominator);

        public static Rational operator *(Rational lhs, int rhs)
            => new(lhs.Numerator * rhs, lhs.Denominator);

        public static Rational operator /(Rational lhs, int rhs)
            => new(lhs.Numerator, lhs.Denominator * rhs);

        public static Rational operator +(int lhs, Rational rhs)
            => new(lhs * rhs.Denominator + rhs.Numerator, rhs.Denominator);

        public static Rational operator -(int lhs, Rational rhs)
            => new(lhs * rhs.Denominator - lhs * rhs.Numerator, rhs.Denominator);

        public static Rational operator *(int lhs, Rational rhs)
            => new(lhs * rhs.Numerator, rhs.Denominator);

        public static Rational operator /(int lhs, Rational rhs)
            => new(lhs * rhs.Denominator, rhs.Numerator);

        public static Rational operator -(Rational rhs)
            => new(-rhs.Numerator, rhs.Denominator);

        public static bool operator ==(Rational lhs, Rational rhs)
            => lhs.Numerator == rhs.Numerator && lhs.Denominator == rhs.Denominator;

        public static bool operator !=(Rational lhs, Rational rhs)
            => !(lhs == rhs);

        public static bool operator >(Rational lhs, Rational rhs)
            => lhs.Numerator * rhs.Denominator > rhs.Numerator * lhs.Denominator;
        public static bool operator >=(Rational lhs, Rational rhs)
            => lhs == rhs || lhs > rhs;

        public static bool operator <(Rational lhs, Rational rhs)
            => rhs >= lhs;

        public static bool operator <=(Rational lhs, Rational rhs)
            => rhs > lhs;

        public static Rational Min(Rational lhs, Rational rhs)
            => lhs < rhs ? lhs : rhs;

        public static Rational Max(Rational lhs, Rational rhs)
            => lhs > rhs ? lhs : rhs;

        public override string ToString() {
            StringBuilder builder = new();
            builder.Append(this.Numerator);
            if (this.Denominator != 1) {
                builder.Append('/');
                builder.Append(this.Denominator);
            }
            return builder.ToString();
        }

        public override bool Equals(object? obj) {
            if (obj is not Rational)
                return false;

            Rational rational = (Rational)obj;
            return this == rational;
        }

        public override int GetHashCode() {
            return this.Numerator.GetHashCode() ^ this.Denominator.GetHashCode();
        }

        private static int GreatestCommonFactor(int a, int b) {
            a = Math.Abs(a);
            b = Math.Abs(b);

            if (a == 0)
                return b;

            if (b == 0)
                return a;

            if (a < b)
                (b, a) = (a, b);

            while (true) {
                int mod = a % b;
                if (mod == 0)
                    return b;

                a = b;
                b = mod;
            }
        }
    }
}
