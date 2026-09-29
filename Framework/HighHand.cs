using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Combinatorics;
using System.Runtime.ExceptionServices;

namespace Framework {
    [DebuggerDisplay("{ToString()}")]
    public class HighHand : IComparable<HighHand>, IEquatable<HighHand> {
        private static readonly Card[] MIN_HAND = new Card[5] {
            new(Rank.TWO, Suit.CLUBS),
            new(Rank.THREE, Suit.CLUBS),
            new(Rank.FOUR, Suit.CLUBS),
            new(Rank.FIVE, Suit.CLUBS),
            new(Rank.SEVEN, Suit.DIAMONDS)
        };

        internal static HighHand Min = Build(new FiveCards(MIN_HAND));
        public FiveCards Cards { get; private set; }
        public HandStrength Strength { get; private set; }

        private HighHand(FiveCards cards, HandStrength strength) {
            Cards = cards;
            Strength = strength;
        }

        public override int GetHashCode() {
            return Strength.GetHashCode();
        }

        public int CompareTo(HighHand? other) {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            return Strength.Value.CompareTo(other.Strength.Value);
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
            return a.Strength.Value > b.Strength.Value ? a : b;
        }

        public static HighHand Build(FiveCards cards) {
            Card[] cardArray = cards.GetCards();

            Dictionary<Rank, int> histogram = new();
            Suit firstSuit = cardArray[0].Suit;
            bool isFlush = true;

            for (int i = 0; i < cardArray.Length; i++) {
                Rank rank = cardArray[i].Rank;
                if (!histogram.TryGetValue(rank, out int count))
                    count = 0;
                histogram[rank] = ++count;

                isFlush &= cardArray[i].Suit == firstSuit;
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

            HandType handType = HandType.HighCard;

            if (straight && isFlush) {
                handType = HandType.StraightFlush;
            }
            else if (histogram[sorted[0]] == 4) {
                handType = HandType.FourOfAKind;
            }
            else if (histogram[sorted[0]] == 3 && histogram[sorted[1]] == 2) {
                handType = HandType.FullHouse;
            }
            else if (isFlush) {
                handType = HandType.Flush;
            }
            else if (straight) {
                handType = HandType.Straight;
            }
            else if (histogram[sorted[0]] == 3) {
                handType = HandType.ThreeOfAKind;
            }
            else if (histogram[sorted[0]] == 2 && histogram[sorted[1]] == 2) {
                handType = HandType.TwoPair;
            }
            else if (histogram[sorted[0]] == 2) {
                handType = HandType.OnePair;
            }

            if (straight && sorted[0].Value == Rank.ACE.Value && sorted[1].Value == Rank.FIVE.Value) {
                sorted = new Rank[] { Rank.FIVE, Rank.FOUR, Rank.THREE, Rank.TWO, Rank.ACE };
            }

            int strength = handType.Value << 20;
            for (int i = 0; i < sorted.Length; i++) {
                strength |= sorted[i].Value << (4 * (4 - i));
            }

            return new HighHand(cards, new HandStrength(strength));
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
