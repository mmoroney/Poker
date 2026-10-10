using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Games
{
    internal static class OmahaShared {
        public static ushort MakeHighHand(Card[] board, Card[] holeCards) {
            ushort best = 0;

            foreach (ushort hand in GetHighHands(board, holeCards))
                best = Math.Max(best, hand);

            return best;
        }

        public static byte? MakeLowHand(Card[] board, Card[] holeCards) {
            byte? bestLow = null;

            foreach (byte hand in GetLowHands(board, holeCards))
                bestLow = (bestLow is null) ? hand : Math.Max((byte)bestLow, hand);

            return bestLow;
        }

        private static IEnumerable<ushort> GetHighHands(Card[] board, Card[] holeCards) {
            return GetCards(board, holeCards).Select(cards => LookupTable.Lookup(new FiveCards(cards)));
        }

        private static IEnumerable<byte> GetLowHands(Card[] board, Card[] holeCards) {
            foreach (Card[] cards in GetCards(board, holeCards)) {
                byte? hand = Encoder.EncodeLow(new FiveCards(cards));
                if (hand is not null) {
                    yield return (byte)hand;
                }
            }
        }

        private static IEnumerable<Card[]> GetCards(Card[] fullBoard, Card[] holeCards) {
            Card[] dest = new Card[5];

            foreach (Card[] result in Chooser.Choose(fullBoard, 0, dest, 0, 3)) {
                foreach (Card[] result2 in Chooser.Choose(holeCards, 0, dest, 3, 2)) {
                    yield return result2;
                }
            }
        }
    }
}
