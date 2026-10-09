using Combinatorics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    public static class Encoder {
        public static ushort Encode(FiveCards cards) {
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

            bool isStraight = IsStraight(sorted, histogram);

            HandType handType = HandType.HighCard;
            ushort encodedRanks = 0;

            if (isStraight) {
                handType = isFlush ? HandType.StraightFlush : HandType.Straight;
                encodedRanks = EncodeStraight(sorted);
            }
            else if (histogram[sorted[0]] == 4) {
                handType = HandType.FourOfAKind;
                encodedRanks = EncodeFourOfAKind(sorted);
            }
            else if (histogram[sorted[0]] == 3) {
                if (histogram[sorted[3]] == 2) {
                    handType = HandType.FullHouse;
                    encodedRanks = EncodeFullHouse(sorted);
                }
                else {
                    handType = HandType.ThreeOfAKind;
                    encodedRanks = EncodeThreeOfAKind(sorted);
                }
            }
            else if (histogram[sorted[0]] == 2) {
                if (histogram[sorted[2]] == 2) {
                    handType = HandType.TwoPair;
                    encodedRanks = EncodeTwoPair(sorted);
                }
                else {
                    handType = HandType.OnePair;
                    encodedRanks = EncodeOnePair(sorted);
                }
            }
            else if (isFlush) {
                handType = HandType.Flush;
                encodedRanks = EncodeHighCard(sorted);
            }
            else {
                handType = HandType.HighCard;
                encodedRanks = EncodeHighCard(sorted);
            }

            return (ushort)(handType.Value << 12 | encodedRanks);
        }

        private static bool IsStraight(Rank[] sorted, Dictionary<Rank, int> histogram) {
            if (histogram.Count != 5) {
                return false;
            }

            if (sorted[1].Value - sorted[4].Value != 3) {
                return false;
            }

            if (sorted[0] == Rank.ACE && sorted[1] == Rank.FIVE) {
                return true;
            }

            return sorted[0].Value - 1 == sorted[1].Value;
        }

        private static ushort EncodeStraight(Rank[] sorted) {
            if (sorted[0] == Rank.ACE && sorted[1] == Rank.FIVE) {
                return Rank.FIVE.Value;
            }

            return sorted[0].Value;
        }

        private static ushort EncodeFourOfAKind(Rank[] sorted) {
            return (ushort)((sorted[0].Value << 4) | sorted[4].Value);
        }

        private static ushort EncodeFullHouse(Rank[] sorted) {
            return (ushort)((sorted[0].Value << 4) | sorted[3].Value);
        }

        private static ushort EncodeThreeOfAKind(Rank[] sorted) {
            return (ushort)((sorted[0].Value << 8) | sorted[3].Value << 4 | sorted[4].Value);  
        }

        private static ushort EncodeTwoPair(Rank[] sorted) {
            return (ushort)((sorted[0].Value << 8) | sorted[2].Value << 4 | sorted[4].Value);
        }
        private static ushort EncodeOnePair(Rank[] sorted) {
            byte[] kickers = new byte[] { sorted[4].Value, sorted[3].Value, sorted[2].Value };
            kickers = Array.ConvertAll(kickers, r => r > sorted[0].Value ? (byte)(r - 1) : r);
            return (ushort)((sorted[0].Value << 8) | BijectiveMapping.Encode(kickers));
        }

        private static ushort EncodeHighCard(Rank[] sorted) {
            byte[] values = Array.ConvertAll(sorted, r => r.Value);
            Array.Reverse(values);
            return (ushort)BijectiveMapping.Encode(values);
        }
    }
}
