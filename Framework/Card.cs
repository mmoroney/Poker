using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Framework {
    [DebuggerDisplay("{ToString()}")]
    public class Card : IEquatable<Card> {
        public Rank Rank { get; private set; }
        public Suit Suit { get; private set; }

        public Card(Rank rank, Suit suit) {
            this.Rank = rank;
            this.Suit = suit;
        }

        public Card(int n) {
            int ranks = Enum.GetValues(typeof(Rank)).Length;
            int suits = Enum.GetValues(typeof(Suit)).Length;

            if (n < 0 || n >= ranks * suits) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            this.Rank = (Rank)(n / suits);
            this.Suit = (Suit)(n % suits);
        }

        public override string ToString() =>
            String.Format("{0}{1}", "23456789TJQKA"[Convert.ToInt32(this.Rank)], "cdhs"[Convert.ToInt32(this.Suit)]);

        public override bool Equals(object? obj) {
            if (obj is not Card card)
                return false;

            return this == card;
        }

        public override int GetHashCode() {
            return this.Rank.GetHashCode() ^ this.Suit.GetHashCode();
        }

        public bool Equals(Card? other) {
            if (other is null)
                return false;

            return this == other;
        }

        public static bool operator ==(Card a, Card b) {
            return a.Rank == b.Rank && a.Suit == b.Suit;
        }

        public static bool operator !=(Card a, Card b) {
            return !(a == b);
        }

        public static Card Parse(string s) {
            if (s.Length != 2)
                throw new ArgumentException(String.Format("The string must contain exactly two characters. String: {0}", s) ,nameof(s));

            int i = "23456789TJQKA".IndexOf(s[0]);
            if (i == -1)
                throw new ArgumentException(String.Format("Unrecognized rank character: {0}", s[0]), nameof(s));

            
            Rank rank = (Rank)i;
            Suit suit = (Suit)Enum.Parse(typeof(Suit), s.AsSpan(1));

            return new Card(rank, suit);
        }
    }
}
