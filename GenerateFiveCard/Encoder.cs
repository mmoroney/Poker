using Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenerateFiveCard {
    public static class Encoder {
        public static HandStrength Encode(FiveCards cards) {
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

            Rank[] sorted = Array.ConvertAll(cardArray, c => c.Rank);
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
            else if (histogram[sorted[0]] == 3 && histogram[sorted[3]] == 2) {
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
            else if (histogram[sorted[0]] == 2 && histogram[sorted[2]] == 2) {
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

            return new HandStrength(strength);
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

