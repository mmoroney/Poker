using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    public readonly record struct HandType(byte Value) {
        public readonly static HandType HighCard = new(0);
        public readonly static HandType OnePair = new(1);
        public readonly static HandType TwoPair = new(2);
        public readonly static HandType ThreeOfAKind = new(3);
        public readonly static HandType Straight = new(4);
        public readonly static HandType Flush = new(5);
        public readonly static HandType FullHouse = new(6);
        public readonly static HandType FourOfAKind = new(7);
        public readonly static HandType StraightFlush = new(8);

        public override string ToString() =>
            Value switch {
                0 => "High Card",
                1 => "One Pair",
                2 => "Two Pair",
                3 => "Three of a Kind",
                4 => "Straight",
                5 => "Flush",
                6 => "Full House",
                7 => "Four of a Kind",
                8 => "Straight Flush",
                _ => throw new ArgumentOutOfRangeException(nameof(Value), "Invalid hand type value."),
            };
    }
}
