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
                        Assert.AreEqual(types[i], hand1.handType);

                        HighHand hand2 = MakeHighHand(types[j]);
                        Assert.AreEqual(types[j], hand2.handType);

                        Assert.IsTrue(hand2 > hand1);
                    }
                }
            }
        }

        private static HighHand MakeHighHand(HandType type)
        {
            switch (type)
            {
                case HandType.OnePair:
                    return Utilities.MakeOnePair();
                case HandType.TwoPair:
                    return Utilities.MakeTwoPair();
                case HandType.ThreeOfAKind:
                    return Utilities.MakeThreeOfAKind();
                case HandType.FullHouse:
                    return Utilities.MakeFullHouse();
                case HandType.FourOfAKind:
                    return Utilities.MakeFourOfAKind();
                case HandType.StraightFlush:
                    return Utilities.MakeStraightFlush();
                default:
                    throw new ArgumentException("Unsupported type", nameof(type));
            }
        }


        [TestMethod]
        public void TestHands()
        {
            TestHand(new Card(Rank.A, Suit.c),
                new Card(Rank.K, Suit.c),
                new Card(Rank.Q, Suit.c),
                new Card(Rank.J, Suit.c),
                new Card(Rank.T, Suit.c),
                HandType.StraightFlush);

            TestHand(new Card(Rank.A, Suit.c),
                new Card(Rank.A, Suit.d),
                new Card(Rank.A, Suit.h),
                new Card(Rank.A, Suit.s),
                new Card(Rank.K, Suit.c),
                HandType.FourOfAKind);

            TestHand(new Card(Rank.A, Suit.c),
                new Card(Rank.A, Suit.d),
                new Card(Rank.A, Suit.h),
                new Card(Rank.K, Suit.c),
                new Card(Rank.K, Suit.d),
                HandType.FullHouse);

            TestHand(new Card(Rank.A, Suit.c),
                new Card(Rank.K, Suit.c),
                new Card(Rank.Q, Suit.c),
                new Card(Rank.J, Suit.c),
                new Card(Rank._9, Suit.c),
                HandType.Flush);

            TestHand(new Card(Rank.A, Suit.c),
                new Card(Rank.K, Suit.d),
                new Card(Rank.Q, Suit.h),
                new Card(Rank.J, Suit.s),
                new Card(Rank.T, Suit.c),
                HandType.Straight);

            TestHand(new Card(Rank.A, Suit.c),
                new Card(Rank._2, Suit.d),
                new Card(Rank._3, Suit.h),
                new Card(Rank._4, Suit.s),
                new Card(Rank._5, Suit.c),
                HandType.Straight);

            TestHand(new Card(Rank.A, Suit.c),
                new Card(Rank.A, Suit.d),
                new Card(Rank.A, Suit.h),
                new Card(Rank.K, Suit.c),
                new Card(Rank.Q, Suit.d),
                HandType.ThreeOfAKind);

            TestHand(new Card(Rank.A, Suit.c),
                new Card(Rank.A, Suit.d),
                new Card(Rank.K, Suit.h),
                new Card(Rank.K, Suit.c),
                new Card(Rank.Q, Suit.d),
                HandType.TwoPair);

            TestHand(new Card(Rank.A, Suit.c),
                new Card(Rank.A, Suit.d),
                new Card(Rank.K, Suit.h),
                new Card(Rank.Q, Suit.c),
                new Card(Rank.J, Suit.d),
                HandType.OnePair);

            TestHand(new Card(Rank.A, Suit.c),
                new Card(Rank.K, Suit.h),
                new Card(Rank.Q, Suit.c),
                new Card(Rank.J, Suit.d),
                new Card(Rank._9, Suit.d),
                HandType.HighCard);
        }

        [TestMethod]
        public void TestHandComparison()
        {
            Card[] card1 = new Card[5];
            Card[] card2 = new Card[5];

            card1[0] = new Card(Rank.A, Suit.c);
            card1[1] = new Card(Rank.K, Suit.c);
            card1[2] = new Card(Rank.Q, Suit.c);
            card1[3] = new Card(Rank.J, Suit.c);
            card1[4] = new Card(Rank.T, Suit.c);

            card2[0] = new Card(Rank.K, Suit.d);
            card2[1] = new Card(Rank.Q, Suit.d);
            card2[2] = new Card(Rank.J, Suit.d);
            card2[3] = new Card(Rank.T, Suit.d);
            card2[4] = new Card(Rank._9, Suit.d);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.A, Suit.c);
            card1[1] = new Card(Rank.A, Suit.d);
            card1[2] = new Card(Rank.A, Suit.h);
            card1[3] = new Card(Rank.A, Suit.s);
            card1[4] = new Card(Rank.K, Suit.c);

            card2[0] = new Card(Rank.Q, Suit.c);
            card2[1] = new Card(Rank.Q, Suit.d);
            card2[2] = new Card(Rank.Q, Suit.h);
            card2[3] = new Card(Rank.Q, Suit.s);
            card2[4] = new Card(Rank.J, Suit.c);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.A, Suit.c);
            card1[1] = new Card(Rank.A, Suit.d);
            card1[2] = new Card(Rank.A, Suit.h);
            card1[3] = new Card(Rank.K, Suit.s);
            card1[4] = new Card(Rank.K, Suit.c);

            card2[0] = new Card(Rank.Q, Suit.c);
            card2[1] = new Card(Rank.Q, Suit.d);
            card2[2] = new Card(Rank.Q, Suit.h);
            card2[3] = new Card(Rank.J, Suit.s);
            card2[4] = new Card(Rank.J, Suit.c);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.A, Suit.c);
            card1[1] = new Card(Rank.K, Suit.d);
            card1[2] = new Card(Rank.Q, Suit.h);
            card1[3] = new Card(Rank.J, Suit.s);
            card1[4] = new Card(Rank._9, Suit.c);

            card2[0] = new Card(Rank.A, Suit.c);
            card2[1] = new Card(Rank.K, Suit.d);
            card2[2] = new Card(Rank.Q, Suit.h);
            card2[3] = new Card(Rank.J, Suit.s);
            card2[4] = new Card(Rank._8, Suit.c);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.A, Suit.c);
            card1[1] = new Card(Rank.A, Suit.d);
            card1[2] = new Card(Rank.A, Suit.h);
            card1[3] = new Card(Rank.K, Suit.s);
            card1[4] = new Card(Rank.Q, Suit.c);

            card2[0] = new Card(Rank.J, Suit.c);
            card2[1] = new Card(Rank.J, Suit.d);
            card2[2] = new Card(Rank.J, Suit.h);
            card2[3] = new Card(Rank.T, Suit.s);
            card2[4] = new Card(Rank._9, Suit.c);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.A, Suit.c);
            card1[1] = new Card(Rank.A, Suit.d);
            card1[2] = new Card(Rank.A, Suit.h);
            card1[3] = new Card(Rank.K, Suit.s);
            card1[4] = new Card(Rank.Q, Suit.c);

            card2[0] = new Card(Rank.J, Suit.c);
            card2[1] = new Card(Rank.J, Suit.d);
            card2[2] = new Card(Rank.J, Suit.h);
            card2[3] = new Card(Rank.T, Suit.s);
            card2[4] = new Card(Rank._9, Suit.c);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.A, Suit.c);
            card1[1] = new Card(Rank.A, Suit.d);
            card1[2] = new Card(Rank.K, Suit.h);
            card1[3] = new Card(Rank.K, Suit.s);
            card1[4] = new Card(Rank.Q, Suit.c);

            card2[0] = new Card(Rank.J, Suit.c);
            card2[1] = new Card(Rank.J, Suit.d);
            card2[2] = new Card(Rank.T, Suit.h);
            card2[3] = new Card(Rank.T, Suit.s);
            card2[4] = new Card(Rank._9, Suit.c);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.A, Suit.c);
            card1[1] = new Card(Rank.A, Suit.d);
            card1[2] = new Card(Rank.K, Suit.h);
            card1[3] = new Card(Rank.Q, Suit.s);
            card1[4] = new Card(Rank.J, Suit.c);

            card2[0] = new Card(Rank.J, Suit.c);
            card2[1] = new Card(Rank.J, Suit.d);
            card2[2] = new Card(Rank.T, Suit.h);
            card2[3] = new Card(Rank._9, Suit.s);
            card2[4] = new Card(Rank._8, Suit.c);

            TestUnequalHandComparison(card1, card2);

            card1[0] = new Card(Rank.A, Suit.c);
            card1[1] = new Card(Rank.K, Suit.d);
            card1[2] = new Card(Rank.Q, Suit.h);
            card1[3] = new Card(Rank.J, Suit.s);
            card1[4] = new Card(Rank._9, Suit.c);

            card2[0] = new Card(Rank._8, Suit.c);
            card2[1] = new Card(Rank._7, Suit.d);
            card2[2] = new Card(Rank._6, Suit.h);
            card2[3] = new Card(Rank._5, Suit.s);
            card2[4] = new Card(Rank._3, Suit.c);

            TestUnequalHandComparison(card1, card2);
        }

        private static void TestHand(Card card1, Card card2, Card card3, Card card4, Card card5, HandType handType)
        {
            Card[] cards = new Card[5];

            cards[0] = card1;
            cards[1] = card2;
            cards[2] = card3;
            cards[3] = card4;
            cards[4] = card5;

            HighHand hand = HighHand.Build(cards);
            Assert.AreEqual(handType, hand.handType);
        }

        private static void TestUnequalHandComparison(Card[] hand1, Card[] hand2)
        {
            HighHand a = HighHand.Build(hand1);
            HighHand b = HighHand.Build(hand2);

            Assert.IsTrue(a > b);
        }
    }
}