using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Combinatorics;
using System.Runtime.ExceptionServices;

namespace Framework {
    public enum HandType {
        HighCard = 0,
        OnePair = 1,
        TwoPair = 2,
        ThreeOfAKind = 3,
        Straight = 4,
        Flush = 5,
        FullHouse = 6,
        FourOfAKind = 7,
        StraightFlush = 8
    }

    [DebuggerDisplay("{ToString()}")]
    public class HighHand : IComparable<HighHand>, IEquatable<HighHand> {
        private const int HIGH_CARD = 0;
        private const int ONE_PAIR = 1;
        private const int TWO_PAIR = 2;
        private const int THREE_OF_A_KIND = 3;
        private const int STRAIGHT = 4;
        private const int FLUSH = 5;
        private const int FULL_HOUSE = 6;
        private const int FOUR_OF_A_KIND = 7;
        private const int STRAIGHT_FLUSH = 8;

        internal static HighHand Min = new(-1, -1);
        private readonly int index;
        private readonly int strength;

        private HighHand(int index, int strength) {
            this.index = index;
            this.strength = strength;
        }

        public HandType HandType {
            get {
                return (HandType)(strength >> 20);
            }
        }

        public override int GetHashCode() {
            return strength;
        }

        public int CompareTo(HighHand? other) {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            return strength.CompareTo(other.strength);
        }

        public override bool Equals(object? obj) {
            if (obj is not HighHand other)
                return false;

            return this.CompareTo(other) == 0;
        }

        public bool Equals(HighHand? other) {
            if (other is null)
                return false;

            return this.CompareTo(other) == 0;
        }

        public static bool operator <(HighHand a, HighHand b) {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(HighHand a, HighHand b) {
            return a.CompareTo(b) > 0;
        }

        public static bool operator ==(HighHand a, HighHand b) {
            return a.CompareTo(b) == 0;
        }

        public static bool operator !=(HighHand a, HighHand b) {
            return a.CompareTo(b) != 0;
        }

        public static HighHand Max(HighHand a, HighHand b) {
            return b < a ? a : b;
        }

        public static HighHand Build(Card[] cards) {
            if (cards.Length != 5) {
                throw new ArgumentException("There must be exactly 5 cards.", nameof(cards));
            }

            Dictionary<Rank, int> histogram = new();
            Suit firstSuit = cards[0].Suit;
            bool isFlush = true;

            for (int i = 0; i < cards.Length; i++) {
                Rank rank = cards[i].Rank;
                if (!histogram.TryGetValue(rank, out int count))
                    count = 0;
                histogram[rank] = ++count;

                isFlush &= cards[i].Suit == firstSuit;
            }

            Rank[] sorted = new Rank[histogram.Count];
            Array.Copy(histogram.Keys.ToArray(), 0, sorted, 0, histogram.Count);
            Array.Sort(sorted, (a, b) => {
                int count1 = histogram[a];
                int count2 = histogram[b];
                if (count1 != count2)
                    return count2.CompareTo(count1);

                return b.Value.CompareTo(a.Value);
            });

            bool straight = IsStraight(sorted, histogram);

            int handType = HIGH_CARD;

            if (straight && isFlush) {
                handType = STRAIGHT_FLUSH;
            }
            else if (histogram[sorted[0]] == 4) {
                handType = FOUR_OF_A_KIND;
            }
            else if (histogram[sorted[0]] == 3 && histogram[sorted[1]] == 2) {
                handType = FULL_HOUSE;
            }
            else if (isFlush) {
                handType = FLUSH;
            }
            else if (straight) {
                handType = STRAIGHT;
            }
            else if (histogram[sorted[0]] == 3) {
                handType = THREE_OF_A_KIND;
            }
            else if (histogram[sorted[0]] == 2 && histogram[sorted[1]] == 2) {
                handType = TWO_PAIR;
            }
            else if (histogram[sorted[0]] == 2) {
                handType = ONE_PAIR;
            }

            int strength = handType << 20;
            if (straight) {
                strength |= (sorted[0].Value == 12 && sorted[1].Value == 3) ? 3 : sorted[0].Value;
            }
            else {
                for (int i = 0; i < sorted.Length; i++) {
                    strength |= sorted[i].Value << (4 * (4 - i));
                }
            }

            return new HighHand(BijectiveMapping.Encode(Array.ConvertAll<Card, int>(cards, r => r.Value)), strength);
        }

        private static bool IsStraight(Rank[] ranks, Dictionary<Rank, int> histogram) {
            if (histogram.Count != 5)
                return false;

            if (ranks[0].Value - ranks[4].Value == 4)
                return true;

            if (ranks[0].Value == 12 && ranks[1].Value == 3) {
                Rank ace = ranks[0];
                for (int i = 0; i < 4; i++)
                    ranks[i] = ranks[i + 1];

                ranks[4] = ace;
                return true;
            }

            return false;
        }
    }
}
