using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    public record class DecodedHand(HandType HandType, Rank[] EncodedRanks) {
    }

    public static class Decoder {
        public static DecodedHand Decode(ushort encoded) {
            var type = new HandType { Value = (byte)(encoded >> 12) };
            var ranks = new Rank[5];
            if (type.Value == HandType.Straight.Value || type.Value == HandType.StraightFlush.Value) {
                ranks = DecodeStraight(encoded, ranks);
            }
            else if (type.Value == HandType.FourOfAKind.Value) {
                ranks = DecodeFourOfAKind(encoded, ranks);
            }
            else if (type.Value == HandType.FullHouse.Value) {
                ranks = DecodeFullHouse(encoded, ranks);
            }
            else if (type.Value == HandType.ThreeOfAKind.Value) {
                ranks = DecodeThreeOfAKind(encoded, ranks);
            }
            else if (type.Value == HandType.TwoPair.Value) {
                ranks = DecodeTwoPair(encoded, ranks);
            }
            else if (type.Value == HandType.OnePair.Value) {
                ranks = DecodeOnePair(encoded, ranks);
            }
            else {
                ranks = DecodeHighCard(encoded, ranks);
            }
            return new DecodedHand(type, ranks);
        }

        private static Rank[] DecodeStraight(ushort encoded, Rank[] ranks) {
            byte rank = (byte)(encoded & 0xF);
            for (int i = 0; i < 4; i++) {
                ranks[i] = new Rank { Value = (byte)(rank - i) };
            }
            ranks[4] = (rank == Rank.FIVE.Value) ? Rank.ACE : new Rank { Value = (byte)(rank - 4) };
            return ranks;
        }
        private static Rank[] DecodeFourOfAKind(ushort encoded, Rank[] ranks) {
            byte rank1 = (byte)((encoded >> 4) & 0xF);
            for (int i = 0; i < 4; i++) {
                ranks[i] = new Rank { Value = rank1 };
            }
            ranks[4] = new Rank { Value = (byte)(encoded & 0xF) };
            return ranks;
        }

        private static Rank[] DecodeFullHouse(ushort encoded, Rank[] ranks) {
            byte threeOfAKindRank = (byte)((encoded >> 4) & 0xF);
            byte pairRank = (byte)(encoded & 0xF);
            for (int i = 0; i < 3; i++) {
                ranks[i] = new Rank { Value = threeOfAKindRank };
            }
            for (int i = 3; i < 5; i++) {
                ranks[i] = new Rank { Value = pairRank };
            }
            return ranks;
        }

        private static Rank[] DecodeThreeOfAKind(ushort encoded, Rank[] ranks) {
            byte threeOfAKindRank = (byte)((encoded >> 8) & 0xF);
            byte kicker1Rank = (byte)((encoded >> 4) & 0xF);
            byte kicker2Rank = (byte)(encoded & 0xF);
            for (int i = 0; i < 3; i++) {
                ranks[i] = new Rank { Value = threeOfAKindRank };
            }
            ranks[3] = new Rank { Value = kicker1Rank };
            ranks[4] = new Rank { Value = kicker2Rank };
            return ranks;
        }

        private static Rank[] DecodeTwoPair(ushort encoded, Rank[] ranks) {
            byte highPairRank = (byte)((encoded >> 8) & 0xF);
            byte lowPairRank = (byte)((encoded >> 4) & 0xF);
            byte kickerRank = (byte)(encoded & 0xF);
            for (int i = 0; i < 2; i++) {
                ranks[i] = new Rank { Value = highPairRank };
            }
            for (int i = 2; i < 4; i++) {
                ranks[i] = new Rank { Value = lowPairRank };
            }
            ranks[4] = new Rank { Value = kickerRank };
            return ranks;
        }

        private static Rank[] DecodeOnePair(ushort encoded, Rank[] ranks) {
            byte pairRank = (byte)((encoded >> 8) & 0xF);
            int code = encoded & 0xFF;
            byte[] kickers = Combinatorics.BijectiveMapping.Decode(code, 3);

            for (int i = 0; i < kickers.Length; i++) {
                if (kickers[i] >= pairRank) kickers[i]++;
            }

            for (int i = 0; i < 2; i++) {
                ranks[i] = new Rank { Value = pairRank };
            }
            ranks[2] = new Rank { Value = kickers[2] };
            ranks[3] = new Rank { Value = kickers[1] };
            ranks[4] = new Rank { Value = kickers[0] };
            return ranks;
        }

        private static Rank[] DecodeHighCard(ushort encoded, Rank[] ranks) {
            // encoded low 12 bits contain the bijective mapping code for the five ranks
            int code = encoded & 0xFFF;
            byte[] vals = Combinatorics.BijectiveMapping.Decode(code, 5);
            Array.Reverse(vals);
            for (int i = 0; i < 5; i++) {
                ranks[i] = new Rank { Value = vals[i] };
            }
            return ranks;
        }

        public static Rank[] DecodeLow(byte encoded) {
            // decode the bijective mapping for low hand indices (0 -> ACE, 1.. -> TWO..)
            byte[] vals = Combinatorics.BijectiveMapping.Decode(encoded, 5);
            var ranks = new Rank[5];
            for (int i = 0; i < vals.Length; i++) {
                byte v = vals[i];
                if (v == 0) {
                    ranks[i] = Rank.ACE;
                }
                else {
                    ranks[i] = new Rank(v - 1);
                }
            }
            return ranks;
        }
    }
}
