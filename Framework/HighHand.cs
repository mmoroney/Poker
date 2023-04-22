using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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
    public class HighHand : Hand, IComparable<HighHand>, IEquatable<HighHand> {
        internal static HighHand Min = new(new Card[] {
            new Card(Rank._2, Suit.c),
            new Card(Rank._2, Suit.c),
            new Card(Rank._2, Suit.c),
            new Card(Rank._2, Suit.c),
            new Card(Rank._2, Suit.c)
        }, HandType.HighCard);

        public readonly HandType handType;

        private HighHand(Card[] cards, HandType handType) : base(cards) {
            this.handType = handType;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public int CompareTo(HighHand? other) {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (this.handType != other.handType)
                return this.handType.CompareTo(other.handType);

            for (int i = 0; i < this.cards.Length; i++) {
                Rank rank1 = this.cards[i].Rank;
                Rank rank2 = other.cards[i].Rank;
                if (rank1 != rank2)
                    return rank1.CompareTo(rank2);
            }

            return 0;
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
            Suit? suit = null;

            for (int i = 0; i < cards.Length; i++) {
                if (i == 0)
                    suit = cards[i].Suit;
                else if (suit != null && suit != cards[i].Suit)
                    suit = null;

                if (!histogram.TryGetValue(cards[i].Rank, out int count))
                    count = 0;

                histogram[cards[i].Rank] = ++count;
            }

            Card[] sorted = new Card[cards.Length];
            Array.Copy(cards, 0, sorted, 0, cards.Length);
            Array.Sort(sorted, (a, b) => {
                int count1 = histogram[a.Rank];
                int count2 = histogram[b.Rank];
                if (count1 != count2)
                    return count2.CompareTo(count1);

                if (a.Rank != b.Rank)
                    return b.Rank.CompareTo(a.Rank);

                return b.Suit.CompareTo(a.Suit);
            });

            bool straight = IsStraight(sorted, histogram);
            bool flush = suit != null;

            HandType handType = HandType.HighCard;

            if (straight && flush)
                handType = HandType.StraightFlush;

            else if (histogram[sorted[0].Rank] == 4)
                handType = HandType.FourOfAKind;

            else if (histogram[sorted[0].Rank] == 3 && histogram[sorted[3].Rank] == 2)
                handType = HandType.FullHouse;

            else if (flush)
                handType = HandType.Flush;

            else if (straight)
                handType = HandType.Straight;

            else if (histogram[sorted[0].Rank] == 3 && histogram[sorted[3].Rank] == 1)
                handType = HandType.ThreeOfAKind;

            else if (histogram[sorted[0].Rank] == 2 && histogram[sorted[2].Rank] == 2)
                handType = HandType.TwoPair;

            else if (histogram[sorted[0].Rank] == 2 && histogram[sorted[2].Rank] == 1)
                handType = HandType.OnePair;

            return new HighHand(sorted, handType);
        }

        private static bool IsStraight(Card[] cards, Dictionary<Rank, int> histogram) {
            if (histogram.Count != 5)
                return false;

            if ((int)cards[0].Rank - (int)cards[4].Rank == 4)
                return true;

            if (cards[0].Rank == Rank.A && cards[1].Rank == Rank._5) {
                Card ace = cards[0];
                for (int i = 0; i < 4; i++)
                    cards[i] = cards[i + 1];

                cards[4] = ace;
                return true;
            }

            return false;
        }
    }
}
