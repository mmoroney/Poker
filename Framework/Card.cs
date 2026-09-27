using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Framework {
    [DebuggerDisplay("{ToString()}")]
    public readonly record struct Card(byte Value) {

        public Card(int value) : this((byte)value) {
            if (value < 0 || value > 51) {
                throw new ArgumentOutOfRangeException(nameof(value), "Card value must be between 0 and 51.");
            }
        }

        public Card(Rank rank, Suit suit) : this((byte)(4 * rank.Value + suit.Value)) {
        }

        public Rank Rank => new(Value / 4);
        public Suit Suit => new(Value % 4);

        public override string ToString() =>
            String.Format("{0}{1}", Rank, Suit);

        public static Card Parse(string s) {
            if (s.Length != 2)
                throw new ArgumentException(String.Format("The string must contain exactly two characters. String: {0}", s) ,nameof(s));

            Rank rank = new("23456789TJQKA".IndexOf(s[0]));
            Suit suit = new("cdhs".IndexOf(s[1]));

            return new(rank, suit);
        }
    }
}
