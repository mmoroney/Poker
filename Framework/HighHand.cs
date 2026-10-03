using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Combinatorics;
using System.Runtime.ExceptionServices;
using System.IO;

namespace Framework {
    [DebuggerDisplay("{ToString()}")]
    public class HighHand : IComparable<HighHand>, IEquatable<HighHand> {
        private static readonly Card[] MIN_HAND = new Card[5] {
            new(Rank.TWO, Suit.CLUBS),
            new(Rank.THREE, Suit.CLUBS),
            new(Rank.FOUR, Suit.CLUBS),
            new(Rank.FIVE, Suit.CLUBS),
            new(Rank.SEVEN, Suit.DIAMONDS)
        };

        // table of strengths loaded from embedded resource Data/five_card.dat
        private static readonly ushort[] FiveCardTable;

        static HighHand()
        {
            var assembly = typeof(HighHand).Assembly;
            // default manifest resource name: <RootNamespace>.Data.five_card.dat
            string resourceName = assembly.GetName().Name + ".Data.five_card.dat";

            using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
            using MemoryStream ms = new();
            stream.CopyTo(ms);
            byte[] bytes = ms.ToArray();

            FiveCardTable = new ushort[bytes.Length / sizeof(ushort)];
            Buffer.BlockCopy(bytes, 0, FiveCardTable, 0, bytes.Length);

            Min = Build(new FiveCards(MIN_HAND));
        }

        internal static HighHand Min;
        public FiveCards Cards { get; private set; }
        public int Strength { get; private set; }

        public HandType HandType {
            get {
                if (Strength < 1277) {
                    return HandType.HighCard;
                }
                if (Strength < 4138) {
                    return HandType.OnePair;
                }
                if (Strength < 4995) {
                    return HandType.TwoPair;
                }
                if (Strength < 5853) {
                    return HandType.ThreeOfAKind;
                }
                if (Strength < 5863) {
                    return HandType.Straight;
                }
                if (Strength < 7140) {
                    return HandType.Flush;
                }
                if (Strength < 7296) {
                    return HandType.FullHouse;
                }
                if (Strength < 7452) {
                    return HandType.FourOfAKind;
                }
                return HandType.StraightFlush;
            }
        }

        private HighHand(FiveCards cards, int strength) {
            Cards = cards;
            Strength = strength;
        }

        public override int GetHashCode() {
            return Strength.GetHashCode();
        }

        public int CompareTo(HighHand? other) {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            return Strength.CompareTo(other.Strength);
        }

        public override bool Equals(object? obj) {
            if (obj is not HighHand other)
                return false;

            return this.CompareTo(other) == 0;
        }

        public bool Equals(HighHand? other) {
            if (other is null)
                return false;

            return this.CompareTo(other) == 0;
        }

        public static bool operator <(HighHand a, HighHand b) {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(HighHand a, HighHand b) {
            return a.CompareTo(b) > 0;
        }

        public static bool operator ==(HighHand a, HighHand b) {
            return a.CompareTo(b) == 0;
        }

        public static bool operator !=(HighHand a, HighHand b) {
            return a.CompareTo(b) != 0;
        }

        public static HighHand Max(HighHand a, HighHand b) {
            return a.Strength > b.Strength ? a : b;
        }

        public static int LookupStrength(FiveCards cards) {
            return FiveCardTable[cards.Value];
        }

        public static HighHand Build(FiveCards cards) {
            int strength = LookupStrength(cards);
            return new HighHand(cards, strength);
        }
    }
}
