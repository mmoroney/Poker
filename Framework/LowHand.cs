using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Combinatorics;

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
            return Build(Array.ConvertAll(cards, c => c.ToInt()));
        }

        public static LowHand? Build(int[] cards) {
            if (cards.Length != 5)
                throw new ArgumentException("cards parameter must have 5 values.", nameof(cards));

            HashSet<int> ranks = new();
            int[] lowRanks = new int[cards.Length];

            foreach (int card in cards) {
                if (card < 0 || card > 51)
                    throw new ArgumentOutOfRangeException(nameof(cards), "Card values must be between 0 and 51.");
                int? rank = ToLowRank(card);
                if (rank is null)
                    return null;

                int r = rank.Value;
                if (ranks.Contains(r))
                    return null;        

                ranks.Add(r);
                lowRanks[ranks.Count - 1] = r;
            }

            Array.Sort(lowRanks);
            

            return new LowHand(BijectiveMapping.Encode(cards), BijectiveMapping.Encode(lowRanks));
        }

        private static int? ToLowRank(int card) {
            int rank = card / 4;
            if (rank == 12)
                return 0;
            if (rank >= 8)
                return null;
            return rank;
        }
    }
}

