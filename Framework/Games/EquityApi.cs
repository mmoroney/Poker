using System;
using System.Linq;

namespace Framework.Games
{
    public static class EquityApi
    {
        public static Rational[] CalculateHoldem(string partialBoard, string[] holeCards)
        {
            return Holdem.CalculateEquity(partialBoard ?? string.Empty, holeCards);
        }

        public static Rational[] CalculateOmahaHigh(string partialBoard, string[] holeCards)
        {
            return OmahaHigh.CalculateEquity(partialBoard ?? string.Empty, holeCards);
        }

        public static Rational[] CalculateOmahaHighLow(string partialBoard, string[] holeCards, int holeCount)
        {
            // parse hole cards with specified holeCount
            Card[][] holeCardsParsed = holeCards.Select(s => Parser.ParseHoleCards(s, holeCount)).ToArray();
            return Equity.CalculateHighLow(Parser.ParsePartialBoard(partialBoard ?? string.Empty), holeCardsParsed);
        }
    }
}
