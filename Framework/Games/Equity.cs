using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Games {
    internal static class Equity {
        public static Rational[] CalculateHigh(Card[] partialBoard, Card[][] holeCards, Func<Card[], Card[], HighHand> makeHand) {
            Card[] deck = Deck.Build(holeCards, partialBoard);
            Rational[] wins = Enumerable.Range(start: 0, count: holeCards.Length)
                .Select(_ => Rational.Zero)
                .ToArray();

            Card[] board = new Card[5];
            partialBoard.CopyTo(board, 0);
            int count = 0;

            foreach (Card[] fullBoard in Chooser.Choose(deck, 0, board, partialBoard.Length, 5 - partialBoard.Length)) {
                count++;
                Rational[] result = GetWinsHigh(fullBoard, holeCards, makeHand);
                for (int i = 0; i < result.Length; i++)
                    wins[i] = wins[i] + result[i];
            }

            return wins.Select(i => i / count).ToArray();
        }

        public static Rational[] CalculateHighLow(Card[] partialBoard, Card[][] holeCards) {
            Card[] deck = Deck.Build(holeCards, partialBoard);
            Rational[] wins = Enumerable.Range(start: 0, count: holeCards.Length)
                .Select(_ => Rational.Zero)
                .ToArray();

            Card[] board = new Card[5];
            partialBoard.CopyTo(board, 0);
            int count = 0;

            foreach (Card[] fullBoard in Chooser.Choose(deck, 0, board, partialBoard.Length, 5 - partialBoard.Length)) {
                count++;
                Rational[] highResult = GetWinsHigh(fullBoard, holeCards, OmahaShared.MakeHighHand);
                Rational[]? lowResult = GetWinsLow(fullBoard, holeCards);

                for (int i = 0; i < holeCards.Length; i++)
                    wins[i] = wins[i] + ((lowResult is null) ? highResult[i] : (highResult[i] + lowResult[i]) / 2);
            }

            return wins.Select(i => i / count).ToArray();
        }

        private static Rational[] GetWinsHigh(Card[] fullBoard, Card[][] holeCards, Func<Card[], Card[], HighHand> makeHand) {
            HashSet<int> winners = new();
            HighHand bestHand = HighHand.Min;

            for (int i = 0; i < holeCards.Length; i++) {
                HighHand currentHand = makeHand(fullBoard, holeCards[i]);
                if (currentHand > bestHand) {
                    bestHand = currentHand;
                    winners.Clear();
                    winners.Add(i);
                }
                else if (currentHand == bestHand)
                    winners.Add(i);
            }


            Rational result = Rational.One / winners.Count;
            Rational[] wins = Enumerable.Range(start: 0, count: holeCards.Length)
                .Select(_ => Rational.Zero)
                .ToArray();

            for (int i = 0; i < holeCards.Length; i++) {
                if (winners.Contains(i))
                    wins[i] = result;
            }

            return wins;
        }

        private static Rational[]? GetWinsLow(Card[] fullBoard, Card[][] holeCards) {
            HashSet<int> winners = new();
            LowHand? bestLow = null;

            for (int i = 0; i < holeCards.Length; i++) {
                LowHand? currentHand = OmahaShared.MakeLowHand(fullBoard, holeCards[i]);
                if (currentHand is null)
                    continue;

                if (bestLow is null || currentHand > bestLow) {
                    bestLow = currentHand;
                    winners.Clear();
                    winners.Add(i);
                }
                else if (currentHand == bestLow)
                    winners.Add(i);
            }

            if (bestLow is null)
                return null;

            Rational result = Rational.One / winners.Count;
            Rational[] wins = Enumerable.Range(start: 0, count: holeCards.Length)
                .Select(_ => Rational.Zero)
                .ToArray();

            for (int i = 0; i < holeCards.Length; i++) {
                if (winners.Contains(i))
                    wins[i] = result;
            }

            return wins;
        }
    }
}
