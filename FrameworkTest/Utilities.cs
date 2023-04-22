using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace FrameworkTest
{
    public static class Utilities
    {
        private static readonly Random random = new();
        private static readonly Rank[] allRanks = Enum.GetValues<Rank>();
        private static readonly Suit[] allSuits = Enum.GetValues<Suit>();
        private static readonly Rank[] straightRanks = allRanks.Where(r => r >= Rank._5).ToArray();
        private static readonly Rank[] lowRanks = allRanks.Where(r => r.IsLowRank()).ToArray();

        public static HighHand MakeOnePair()
        {
            Rank[] ranks = ChooseRandom(allRanks, 4);
            Card[] cards = new Card[5];

            Suit[] suits = ChooseRandom(allSuits, 2);
            for (int i = 0; i < 2; i++)
            {
                cards[i] = new Card(ranks[0], suits[i]);
            }

            for (int i = 2; i < cards.Length; i++)
            {
                cards[i] = new Card(ranks[i - 1], ChooseRandom(allSuits));
            }

            return HighHand.Build(cards);
        }

        public static HighHand MakeTwoPair()
        {
            Rank[] ranks = ChooseRandom(allRanks, 3);
            Card[] cards = new Card[5];

            Suit[] suits = ChooseRandom(allSuits, 2).Concat(ChooseRandom(allSuits, 2)).ToArray();
            for (int i = 0; i < 2; i++)
            {
                cards[i] = new Card(ranks[0], suits[i]);
            }

            for (int i = 2; i < 4; i++)
            {
                cards[i] = new Card(ranks[1], suits[i]);
            }

            cards[4] = new Card(ranks[2], ChooseRandom(allSuits));

            return HighHand.Build(cards);
        }

        public static HighHand MakeThreeOfAKind()
        {
            Rank[] ranks = ChooseRandom(allRanks, 3);
            Card[] cards = new Card[5];

            Suit[] suits = ChooseRandom(allSuits, 3);
            for (int i = 0; i < 3; i++)
            {
                cards[i] = new Card(ranks[0], suits[i]);
            }

            cards[3] = new Card(ranks[1], ChooseRandom(allSuits));
            cards[4] = new Card(ranks[2], ChooseRandom(allSuits));

            return HighHand.Build(cards);
        }

        public static HighHand MakeFullHouse()
        {
            Rank[] ranks = ChooseRandom(allRanks, 2);
            Card[] cards = new Card[5];

            Suit[] suits = ChooseRandom(allSuits, 3).Concat(ChooseRandom(allSuits, 2)).ToArray();
            for (int i = 0; i < 3; i++)
            {
                cards[i] = new Card(ranks[0], suits[i]);
            }

            for (int i = 3; i < cards.Length; i++)
            {
                cards[i] = new Card(ranks[1], suits[i]);
            }

            return HighHand.Build(cards);
        }

        public static HighHand MakeFourOfAKind()
        {
            Rank[] ranks = ChooseRandom(allRanks, 2);
            Card[] cards = new Card[5];
            for (int i = 0; i < 4; i++)
            {
                cards[i] = new Card(ranks[0], allSuits[i]);
            }

            cards[4] = new Card(ranks[1], ChooseRandom(allSuits));
            return HighHand.Build(cards);
        }

        public static HighHand MakeStraightFlush()
        {
            Suit suit = ChooseRandom(allSuits);
            Rank rank = ChooseRandom(straightRanks);

            Card[] cards = new Card[5];
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = new Card(rank, suit);
                int n = (int)rank - 1;
                rank = (n < 0) ? Rank.A : (Rank)n;
            }

            return HighHand.Build(cards);
        }

        public static LowHand MakeLowHand()
        {
            Rank[] ranks = ChooseRandom(lowRanks, 5);
            Card[] cards = new Card[5];

            for (int i = 0; i < cards.Length;i++)
            {
                cards[i] = new Card(ranks[i], ChooseRandom(allSuits));
            }

            LowHand? lowHand = LowHand.Build(cards);

            if (lowHand is null)
            {
                throw new InvalidOperationException();
            }

            return lowHand;
        }

        public static T ChooseRandom<T>(T[] values)
        {
            return values[random.Next(values.Length)];
        }

        public static T[] ChooseRandom<T>(T[] values, int count)
        {
            T[] result = new T[count];
            ChooseRandom(values, 0, result, 0);
            return result;
        }

        private static void ChooseRandom<T>(T[] values, int valueIndex, T[] result, int resultIndex)
        {
            int count = result.Length - resultIndex;
            if (count == 0)
            {
                return;
            }

            int remaining = values.Length - 1 - valueIndex;

            if (random.Next(remaining) < count)
            {
                result[resultIndex] = values[valueIndex];
                resultIndex++;
            }

            ChooseRandom(values, valueIndex + 1, result, resultIndex);
        }
    }
}
