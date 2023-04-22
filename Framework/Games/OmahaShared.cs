using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Games
{
    internal static class OmahaShared {
        public static HighHand MakeHighHand(Card[] board, Card[] holeCards) {
            HighHand best = HighHand.Min;

            foreach (HighHand hand in GetHighHands(board, holeCards))
                best = HighHand.Max(best, hand);

            return best;
        }

        public static LowHand? MakeLowHand(Card[] board, Card[] holeCards) {
            LowHand? bestLow = null;

            foreach (LowHand hand in GetLowHands(board, holeCards))
                bestLow = (bestLow is null) ? hand : LowHand.Max(bestLow, hand);

            return bestLow;
        }

        private static IEnumerable<HighHand> GetHighHands(Card[] board, Card[] holeCards) {
            return GetCards(board, holeCards).Select(cards => HighHand.Build(cards));
        }

        private static IEnumerable<LowHand> GetLowHands(Card[] board, Card[] holeCards) {
            foreach (Card[] cards in GetCards(board, holeCards)) {
                LowHand? hand = LowHand.Build(cards);
                if (hand is not null) {
                    yield return hand;
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
