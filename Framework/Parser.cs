using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    internal static class Parser {
        public static Card[] ParseFullBoard(string s) {
            if (s.Length != 10)
                throw new ArgumentException(string.Format("A full board must contain exactly 5 cards. {0}", s), nameof(s));

            return ParseCards(s);
        }

        public static Card[] ParsePartialBoard(string s) {
            if (s.Length > 10)
                throw new ArgumentException(string.Format("A board cannot contain more than 5 cards. {0}", s), nameof(s));

            if (s.Length %2 != 0)
                throw new ArgumentException(string.Format("A board cannot contain partial cards. {0}", s), nameof(s));

            return ParseCards(s);
        }

        public static Card[] ParseHoleCards(string s, int expectedCount) {
            if (s.Length != expectedCount * 2)
                throw new ArgumentException(string.Format("There must be exactly {0} hole cards must contain exactly 6 cards. {1}", expectedCount, s), nameof(s));

            return ParseCards(s);
        }

        private static Card[] ParseCards(string s) {
            Card[] cards = new Card[s.Length / 2];

            for (int i = 0; i < cards.Length; i++)
                cards[i] = Card.Parse(s.Substring(2 * i, 2));

            return cards;
        }
    }
}
