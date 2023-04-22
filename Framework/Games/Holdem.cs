using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Games
{
    public static class Holdem
    {
        public static Rational[] CalculateEquity(string partialBoard, string[] holeCards) {
            Card[][] holeCardsParsed = holeCards.Select(s => Parser.ParseHoleCards(s, 2)).ToArray();
            return Equity.CalculateHigh(Parser.ParsePartialBoard(partialBoard), holeCardsParsed, MakeHoldemHand);
        }

        private static HighHand MakeHoldemHand(Card[] board, Card[] holeCards) {
            HighHand best = HighHand.Min;

            Card[] source = board.Concat(holeCards).ToArray();
            Card[] dest = new Card[5];

            foreach (Card[] cards in Chooser.Choose(source, dest))
                best = HighHand.Max(best, HighHand.Build(cards));

            return best;
        }
    }
}
