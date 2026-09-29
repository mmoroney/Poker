using Combinatorics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    [DebuggerDisplay("{ToString()}")]
    public class LowHand : IComparable<LowHand>, IEquatable<LowHand> {
        private readonly FiveCards cards;
        private readonly int strength;
        private LowHand(FiveCards cards, int strength) {
            this.cards = cards;
            this.strength = strength;
        }

        public Card[] Cards() {
            return cards.GetCards();
        }

        public int CompareTo(LowHand? other) {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            return other.strength.CompareTo(strength);
        }

        public override bool Equals(object? obj) {
            if (obj is not LowHand other)
                return false;

            return CompareTo(other) == 0;
        }

        public override int GetHashCode() {
            return this.strength;
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

        public static LowHand? Build(FiveCards cards) {
            Card[] cardArray = cards.GetCards();

            HashSet<Rank> ranks = new();
            byte[] lowRanks = new byte[cardArray.Length];

            foreach (Card card in cardArray) {
                Rank rank = card.Rank;
                if (!rank.IsLowRank()) { 
                    return null;
                }

                if (ranks.Contains(rank))
                    return null;        

                ranks.Add(rank);
                lowRanks[ranks.Count - 1] = ToLowIndex(rank);
            }

            Array.Sort(lowRanks);

            return new LowHand(cards, BijectiveMapping.Encode(lowRanks));
        }

        private static byte ToLowIndex(Rank rank) {
            if(rank.Value == 12)
                return 0;

            return (byte)(rank.Value + 1);
        }
    }
}

