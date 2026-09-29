using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    [DebuggerDisplay("{ToString()}")]
    public readonly record struct FiveCards(int Value) {
        public FiveCards(Card[] cards) : this(GetIndex(cards)) {
        }

        public Card[] GetCards() {
            byte[] cardValues = Combinatorics.BijectiveMapping.Decode(Value, 5);
            return cardValues.Select(v => new Card(v)).ToArray();
        }

        public override string ToString() {
            return string.Join("", GetCards().Select(c => c.ToString()));
        }

        private static int GetIndex(Card[] cards) {
            if (cards.Length != 5) {
                throw new ArgumentException("FiveCards must contain exactly 5 cards.", nameof(cards));
            }

            Card[] sorted = new Card[cards.Length];
            Array.Copy(cards, 0, sorted, 0, cards.Length);
            Array.Sort(sorted, (a, b) => a.Value.CompareTo(b.Value));
            return Combinatorics.BijectiveMapping.Encode(sorted.Select(c => c.Value).ToArray());
        }
    }
}
