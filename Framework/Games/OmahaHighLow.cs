using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Games
{
    public static class OmahaHighLow {
        public static Rational[] CalculateEquity(string partialBoard, string[] holeCards) {
            Card[][] holeCardsParsed = holeCards.Select(s => Parser.ParseHoleCards(s, 4)).ToArray();
            return Equity.CalculateHighLow(Parser.ParsePartialBoard(partialBoard), holeCardsParsed);
        }
    }
}
