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
        private readonly int index;
        private readonly int strength;
        private LowHand(int index, int strength) {
            this.index = index;
            this.strength = strength;
        }

        public Card[] Cards() {
            int[] decoded = BijectiveMapping.Decode(index, 5);
            return Array.ConvertAll(decoded, card => new Card(card));
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

        public static LowHand? Build(Card[] cards) {
            if (cards.Length != 5)
                throw new ArgumentException("cards parameter must have 5 values.", nameof(cards));

            HashSet<Rank> ranks = new();
            int[] lowRanks = new int[cards.Length];

            foreach (Card card in cards) {
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

            return new LowHand(BijectiveMapping.Encode(Array.ConvertAll<Card, int>(cards, r => r.Value)), BijectiveMapping.Encode(lowRanks));
        }

        private static int ToLowIndex(Rank rank) {
            if(rank.Value == 12)
                return 0;

            return rank.Value + 1;
        }
    }
}

