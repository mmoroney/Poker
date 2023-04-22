using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    [DebuggerDisplay("{ToString()}")]
    public class LowHand : Hand, IComparable<LowHand>, IEquatable<LowHand> {
        private class LowComparer : IComparer<Card> {
            public int Compare(Card? x, Card? y) {
                if (x is null)
                    throw new ArgumentNullException(nameof(x));

                if (y is null)
                    throw new ArgumentNullException(nameof(y));

                return y.Rank.LowComparable().CompareTo(x.Rank.LowComparable());
            }
        }


        private LowHand(Card[] cards) : base(cards) {
        }

        public int CompareTo(LowHand? other) {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            LowComparer comparer = new();

            for (int i = 0; i < this.cards.Length; i++) {
                if (this.cards[i].Rank != other.cards[i].Rank)
                    return comparer.Compare(this.cards[i], other.cards[i]);
            }

            return 0;
        }

        public override bool Equals(object? obj) {
            if (obj is not LowHand other)
                return false;

            return this.CompareTo(other) == 0;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public bool Equals(LowHand? other) {
            if (other is null)
                return false;
            return this.CompareTo(other) == 0;
        }

        public static bool operator <(LowHand a, LowHand b) {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(LowHand a, LowHand b) {
            return a.CompareTo(b) > 0;
        }

        public static bool operator ==(LowHand a, LowHand b) {
            return a.CompareTo(b) == 0;
        }

        public static bool operator !=(LowHand a, LowHand b) {
            return a.CompareTo(b) != 0;
        }
        public static LowHand Max(LowHand a, LowHand b) {
            return b < a ? a : b;
        }

        public static LowHand? Build(Card[] cards) {
            if (cards.Length != 5)
                throw new ArgumentException("cards parameter must have 5 values.", nameof(cards));

            HashSet<Rank> ranks = new();

            foreach (Card card in cards) {
                if (ranks.Contains(card.Rank) || !card.Rank.IsLowRank())
                    return null;

                ranks.Add(card.Rank);
            }

            Card[] sorted = new Card[cards.Length];
            Array.Copy(cards, 0, sorted, 0, cards.Length);
            Array.Sort(sorted, new LowComparer());

            return new LowHand(sorted);
        }
    }
}

