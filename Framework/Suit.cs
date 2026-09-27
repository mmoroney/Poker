using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    [DebuggerDisplay("{ToString()}")]
    public readonly record struct Suit(byte Value) {
        public static readonly Suit CLUBS = new(0);
        public static readonly Suit DIAMONDS = new(1);
        public static readonly Suit HEARTS = new(2);
        public static readonly Suit SPADES = new(3);

        public Suit(int value) : this((byte)value) {
            if (value < 0 || value > 3) {
                throw new ArgumentOutOfRangeException(nameof(value), "Suit must be between 0 and 3.");
            }
        }
        public override string ToString() {
            return "cdhs"[Value].ToString();
        }
    }
}
