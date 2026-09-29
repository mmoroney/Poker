using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework;
using System;

namespace FrameworkTest
{
    [TestClass]
    public class HighHandTest
    {
        [TestMethod]
        public void TestHighHandComparison()
        {
            HandType[] types = new HandType[]
            {
                HandType.OnePair,
                HandType.TwoPair,
                HandType.ThreeOfAKind,
                HandType.FullHouse,
                HandType.FourOfAKind,
                HandType.StraightFlush
            };

            for (int n = 0; n < 100; n++)
            {
                for (int i = 0; i < types.Length; i++)
                {
                    for (int j = i + 1; j < types.Length; j++)
                    {
                        HighHand hand1 = MakeHighHand(types[i]);
                        Assert.AreEqual(types[i], hand1.Strength.HandType);

                        HighHand hand2 = MakeHighHand(types[j]);
                        Assert.AreEqual(types[j], hand2.Strength.HandType);

                        Assert.IsTrue(hand2 > hand1);
                    }
                }
            }
        }

        private static HighHand MakeHighHand(HandType type)
        {
            if (type == HandType.OnePair) return Utilities.MakeOnePair();
            if (type == HandType.TwoPair) return Utilities.MakeTwoPair();
            if (type == HandType.ThreeOfAKind) return Utilities.MakeThreeOfAKind();
            if (type == HandType.FullHouse) return Utilities.MakeFullHouse();
            if (type == HandType.FourOfAKind) return Utilities.MakeFourOfAKind();
            if (type == HandType.StraightFlush) return Utilities.MakeStraightFlush();

            throw new ArgumentException("Unsupported type", nameof(type));
        }


        [TestMethod]
        public void TestHands()
        {
            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.KING, Suit.CLUBS),
                new Card(Rank.QUEEN, Suit.CLUBS),
                new Card(Rank.JACK, Suit.CLUBS),
                new Card(Rank.TEN, Suit.CLUBS),
                HandType.StraightFlush);

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.ACE, Suit.DIAMONDS),
                new Card(Rank.ACE, Suit.HEARTS),
                new Card(Rank.ACE, Suit.SPADES),
                new Card(Rank.KING, Suit.CLUBS),
                HandType.FourOfAKind);

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.ACE, Suit.DIAMONDS),
                new Card(Rank.ACE, Suit.HEARTS),
                new Card(Rank.KING, Suit.CLUBS),
                new Card(Rank.KING, Suit.DIAMONDS),
                HandType.FullHouse);

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.KING, Suit.CLUBS),
                new Card(Rank.QUEEN, Suit.CLUBS),
                new Card(Rank.JACK, Suit.CLUBS),
                new Card(Rank.NINE, Suit.CLUBS),
                HandType.Flush);

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.KING, Suit.DIAMONDS),
                new Card(Rank.QUEEN, Suit.HEARTS),
                new Card(Rank.JACK, Suit.SPADES),
                new Card(Rank.TEN, Suit.CLUBS),
                HandType.Straight);

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.TWO, Suit.DIAMONDS),
                new Card(Rank.THREE, Suit.HEARTS),
                new Card(Rank.FOUR, Suit.SPADES),
                new Card(Rank.FIVE, Suit.CLUBS),
                HandType.Straight);

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.ACE, Suit.DIAMONDS),
                new Card(Rank.ACE, Suit.HEARTS),
                new Card(Rank.KING, Suit.CLUBS),
                new Card(Rank.QUEEN, Suit.DIAMONDS),
                HandType.ThreeOfAKind);

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.ACE, Suit.DIAMONDS),
                new Card(Rank.KING, Suit.HEARTS),
                new Card(Rank.KING, Suit.CLUBS),
                new Card(Rank.QUEEN, Suit.DIAMONDS),
                HandType.TwoPair);

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.ACE, Suit.DIAMONDS),
                new Card(Rank.KING, Suit.HEARTS),
                new Card(Rank.QUEEN, Suit.CLUBS),
                new Card(Rank.JACK, Suit.DIAMONDS),
                HandType.OnePair);

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.KING, Suit.HEARTS),
                new Card(Rank.QUEEN, Suit.CLUBS),
                new Card(Rank.JACK, Suit.DIAMONDS),
                new Card(Rank.NINE, Suit.DIAMONDS),
                HandType.HighCard);
        }

        [TestMethod]
        public void TestHandComparison()
        {
            Card[] card1 = new Card[5];
            Card[] card2 = new Card[5];

            card1[0] = new Card(Rank.ACE, Suit.CLUBS);
            card1[1] = new Card(Rank.KING, Suit.CLUBS);
            card1[2] = new Card(Rank.QUEEN, Suit.CLUBS);
            card1[3] = new Card(Rank.JACK, Suit.CLUBS);
            card1[4] = new Card(Rank.TEN, Suit.CLUBS);

            card2[0] = new Card(Rank.KING, Suit.DIAMONDS);
            card2[1] = new Card(Rank.QUEEN, Suit.DIAMONDS);
            card2[2] = new Card(Rank.JACK, Suit.DIAMONDS);
            card2[3] = new Card(Rank.TEN, Suit.DIAMONDS);
            card2[4] = new Card(Rank.NINE, Suit.DIAMONDS);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.ACE, Suit.CLUBS);
            card1[1] = new Card(Rank.ACE, Suit.DIAMONDS);
            card1[2] = new Card(Rank.ACE, Suit.HEARTS);
            card1[3] = new Card(Rank.ACE, Suit.SPADES);
            card1[4] = new Card(Rank.KING, Suit.CLUBS);

            card2[0] = new Card(Rank.QUEEN, Suit.CLUBS);
            card2[1] = new Card(Rank.QUEEN, Suit.DIAMONDS);
            card2[2] = new Card(Rank.QUEEN, Suit.HEARTS);
            card2[3] = new Card(Rank.QUEEN, Suit.SPADES);
            card2[4] = new Card(Rank.JACK, Suit.CLUBS);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.ACE, Suit.CLUBS);
            card1[1] = new Card(Rank.ACE, Suit.DIAMONDS);
            card1[2] = new Card(Rank.ACE, Suit.HEARTS);
            card1[3] = new Card(Rank.KING, Suit.SPADES);
            card1[4] = new Card(Rank.KING, Suit.CLUBS);

            card2[0] = new Card(Rank.QUEEN, Suit.CLUBS);
            card2[1] = new Card(Rank.QUEEN, Suit.DIAMONDS);
            card2[2] = new Card(Rank.QUEEN, Suit.HEARTS);
            card2[3] = new Card(Rank.JACK, Suit.SPADES);
            card2[4] = new Card(Rank.JACK, Suit.CLUBS);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.ACE, Suit.CLUBS);
            card1[1] = new Card(Rank.KING, Suit.DIAMONDS);
            card1[2] = new Card(Rank.QUEEN, Suit.HEARTS);
            card1[3] = new Card(Rank.JACK, Suit.SPADES);
            card1[4] = new Card(Rank.NINE, Suit.CLUBS);

            card2[0] = new Card(Rank.ACE, Suit.CLUBS);
            card2[1] = new Card(Rank.KING, Suit.DIAMONDS);
            card2[2] = new Card(Rank.QUEEN, Suit.HEARTS);
            card2[3] = new Card(Rank.JACK, Suit.SPADES);
            card2[4] = new Card(Rank.EIGHT, Suit.CLUBS);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.ACE, Suit.CLUBS);
            card1[1] = new Card(Rank.ACE, Suit.DIAMONDS);
            card1[2] = new Card(Rank.ACE, Suit.HEARTS);
            card1[3] = new Card(Rank.KING, Suit.SPADES);
            card1[4] = new Card(Rank.QUEEN, Suit.CLUBS);

            card2[0] = new Card(Rank.JACK, Suit.CLUBS);
            card2[1] = new Card(Rank.JACK, Suit.DIAMONDS);
            card2[2] = new Card(Rank.JACK, Suit.HEARTS);
            card2[3] = new Card(Rank.TEN, Suit.SPADES);
            card2[4] = new Card(Rank.NINE, Suit.CLUBS);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.ACE, Suit.CLUBS);
            card1[1] = new Card(Rank.ACE, Suit.DIAMONDS);
            card1[2] = new Card(Rank.ACE, Suit.HEARTS);
            card1[3] = new Card(Rank.KING, Suit.SPADES);
            card1[4] = new Card(Rank.QUEEN, Suit.CLUBS);

            card2[0] = new Card(Rank.JACK, Suit.CLUBS);
            card2[1] = new Card(Rank.JACK, Suit.DIAMONDS);
            card2[2] = new Card(Rank.JACK, Suit.HEARTS);
            card2[3] = new Card(Rank.TEN, Suit.SPADES);
            card2[4] = new Card(Rank.NINE, Suit.CLUBS);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.ACE, Suit.CLUBS);
            card1[1] = new Card(Rank.ACE, Suit.DIAMONDS);
            card1[2] = new Card(Rank.KING, Suit.HEARTS);
            card1[3] = new Card(Rank.KING, Suit.SPADES);
            card1[4] = new Card(Rank.QUEEN, Suit.CLUBS);

            card2[0] = new Card(Rank.JACK, Suit.CLUBS);
            card2[1] = new Card(Rank.JACK, Suit.DIAMONDS);
            card2[2] = new Card(Rank.TEN, Suit.HEARTS);
            card2[3] = new Card(Rank.TEN, Suit.SPADES);
            card2[4] = new Card(Rank.NINE, Suit.CLUBS);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.ACE, Suit.CLUBS);
            card1[1] = new Card(Rank.ACE, Suit.DIAMONDS);
            card1[2] = new Card(Rank.KING, Suit.HEARTS);
            card1[3] = new Card(Rank.QUEEN, Suit.SPADES);
            card1[4] = new Card(Rank.JACK, Suit.CLUBS);

            card2[0] = new Card(Rank.JACK, Suit.CLUBS);
            card2[1] = new Card(Rank.JACK, Suit.DIAMONDS);
            card2[2] = new Card(Rank.TEN, Suit.HEARTS);
            card2[3] = new Card(Rank.NINE, Suit.SPADES);
            card2[4] = new Card(Rank.EIGHT, Suit.CLUBS);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.ACE, Suit.CLUBS);
            card1[1] = new Card(Rank.ACE, Suit.DIAMONDS);
            card1[2] = new Card(Rank.KING, Suit.HEARTS);
            card1[3] = new Card(Rank.JACK, Suit.SPADES);
            card1[4] = new Card(Rank.NINE, Suit.CLUBS);

            card2[0] = new Card(Rank.EIGHT, Suit.CLUBS);
            card2[1] = new Card(Rank.SEVEN, Suit.DIAMONDS);
            card2[2] = new Card(Rank.SIX, Suit.HEARTS);
            card2[3] = new Card(Rank.FIVE, Suit.SPADES);
            card2[4] = new Card(Rank.THREE, Suit.CLUBS);

            TestUnequalHandComparison(card1, card2);
        }

        private static void TestHand(Card card1, Card card2, Card card3, Card card4, Card card5, HandType handType)
        {
            HighHand hand = HighHand.Build(new FiveCards(new Card[] { card1, card2, card3, card4, card5 }));
            Assert.AreEqual(handType, hand.Strength.HandType);
        }

        private static void TestUnequalHandComparison(Card[] hand1, Card[] hand2)
        {
            HighHand a = HighHand.Build(new FiveCards(hand1));
            HighHand b = HighHand.Build(new FiveCards(hand2));

            Assert.IsTrue(a > b);
        }
    }
}