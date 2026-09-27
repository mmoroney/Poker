using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    [DebuggerDisplay("{ToString()}")]
    public readonly record struct Rank(byte Value) {
        public static readonly Rank TWO = new(0);
        public static readonly Rank THREE = new(1);
        public static readonly Rank FOUR = new(2);
        public static readonly Rank FIVE = new(3);
        public static readonly Rank SIX = new(4);
        public static readonly Rank SEVEN = new(5);
        public static readonly Rank EIGHT = new(6);
        public static readonly Rank NINE = new(7);
        public static readonly Rank TEN = new(8);
        public static readonly Rank JACK = new(9);
        public static readonly Rank QUEEN = new(10);
        public static readonly Rank KING = new(11);
        public static readonly Rank ACE = new(12);

        public Rank(int value) : this((byte)value) {
            if (value < 0 || value > 12) {
                throw new ArgumentOutOfRangeException(nameof(value), "Rank must be between 0 and 12.");
            }
        }

        public override string ToString() {
            return "23456789TJQKA"[Value].ToString();
        }
    }
}
