using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    public abstract class Hand {
        public readonly Card[] cards;

        protected Hand(Card[] cards) {
            if (cards.Length != 5) {
                throw new ArgumentException("cards parameter must contain exactly 5 values", nameof(cards));
            }

            this.cards = cards;
        }

        public override string ToString() => string.Join<Card>("", this.cards);

        public override int GetHashCode() {
            int hash = 0;
            foreach (Card card in cards) {
                hash ^= card.GetHashCode();
            }

            return hash;
        }
    }
}
