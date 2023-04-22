using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    public static class Deck {
        public static Card[] Build(Card[][] hands, Card[] board) {
            HashSet<Card> set = new();

            foreach (Card[] hand in hands) {
                foreach (Card card in hand) {
                    if (set.Contains(card))
                        throw new ArgumentException(string.Format("Duplicate card {0}", card));

                    set.Add(card);
                }
            }

            foreach (Card card in board) {
                if (set.Contains(card))
                    throw new ArgumentException(string.Format("Duplicate card {0}", card));

                set.Add(card);
            }

            List<Card> deck = new();

            for (int i = 0; i < 52; i++) {
                Card card = new(i);
                if (!set.Contains(card))
                    deck.Add(card);
            }

            return deck.ToArray();
        }
    }
}
