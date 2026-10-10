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

        private static ushort MakeHoldemHand(Card[] board, Card[] holeCards) {
            ushort best = 0;

            Card[] source = board.Concat(holeCards).ToArray();
            foreach (Card[] cards in Chooser.Choose(source, 5))
                best = Math.Max(best, LookupTable.Lookup(new FiveCards(cards)));

            return best;
        }
    }
}
