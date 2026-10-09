using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework;
using System.Linq;

namespace FrameworkTest
{
    [TestClass]
    public class EncodeDecodeTests
    {
        [TestMethod]
        public void EncodeThenDecode_ProducesExpectedHandTypeAndRanks()
        {
            void TestHand(Card c1, Card c2, Card c3, Card c4, Card c5, HandType expectedHandType, Rank[] expectedRanks)
            {
                Card[] cards = new[] { c1, c2, c3, c4, c5 };
                FiveCards five = new(cards);

                int encoded = Encoder.Encode(five);
                var decoded = Decoder.Decode((ushort)encoded);

                Assert.AreEqual(expectedHandType, decoded.HandType);

                var expectedVals = expectedRanks.Select(r => r.Value).ToArray();
                var decodedVals = decoded.EncodedRanks.Select(r => r.Value).ToArray();
                CollectionAssert.AreEqual(expectedVals, decodedVals);
            }

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.KING, Suit.CLUBS),
                new Card(Rank.QUEEN, Suit.CLUBS),
                new Card(Rank.JACK, Suit.CLUBS),
                new Card(Rank.TEN, Suit.CLUBS),
                HandType.StraightFlush,
                new Rank[] { Rank.ACE, Rank.KING, Rank.QUEEN, Rank.JACK, Rank.TEN }); // StraightFlush

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.ACE, Suit.DIAMONDS),
                new Card(Rank.ACE, Suit.HEARTS),
                new Card(Rank.ACE, Suit.SPADES),
                new Card(Rank.KING, Suit.CLUBS),
                HandType.FourOfAKind,
                new Rank[] { Rank.ACE, Rank.ACE, Rank.ACE, Rank.ACE, Rank.KING }); // Four of a kind

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.ACE, Suit.DIAMONDS),
                new Card(Rank.ACE, Suit.HEARTS),
                new Card(Rank.KING, Suit.CLUBS),
                new Card(Rank.KING, Suit.DIAMONDS),
                HandType.FullHouse,
                new Rank[] { Rank.ACE, Rank.ACE, Rank.ACE, Rank.KING, Rank.KING }); // Full House

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.KING, Suit.CLUBS),
                new Card(Rank.QUEEN, Suit.CLUBS),
                new Card(Rank.JACK, Suit.CLUBS),
                new Card(Rank.NINE, Suit.CLUBS),
                HandType.Flush,
                new Rank[] { Rank.ACE, Rank.KING, Rank.QUEEN, Rank.JACK, Rank.NINE }); // Flush

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.KING, Suit.DIAMONDS),
                new Card(Rank.QUEEN, Suit.HEARTS),
                new Card(Rank.JACK, Suit.SPADES),
                new Card(Rank.TEN, Suit.CLUBS),
                HandType.Straight,
                new Rank[] { Rank.ACE, Rank.KING, Rank.QUEEN, Rank.JACK, Rank.TEN }); // Straight

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.TWO, Suit.DIAMONDS),
                new Card(Rank.THREE, Suit.HEARTS),
                new Card(Rank.FOUR, Suit.SPADES),
                new Card(Rank.FIVE, Suit.CLUBS),
                HandType.Straight,
                new Rank[] { Rank.FIVE, Rank.FOUR, Rank.THREE, Rank.TWO, Rank.ACE }); // Wheel straight

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.ACE, Suit.DIAMONDS),
                new Card(Rank.ACE, Suit.HEARTS),
                new Card(Rank.KING, Suit.CLUBS),
                new Card(Rank.QUEEN, Suit.DIAMONDS),
                HandType.ThreeOfAKind,
                new Rank[] { Rank.ACE, Rank.ACE, Rank.ACE, Rank.KING, Rank.QUEEN }); // Three of a kind

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.ACE, Suit.DIAMONDS),
                new Card(Rank.KING, Suit.HEARTS),
                new Card(Rank.KING, Suit.CLUBS),
                new Card(Rank.QUEEN, Suit.DIAMONDS),
                HandType.TwoPair,
                new Rank[] { Rank.ACE, Rank.ACE, Rank.KING, Rank.KING, Rank.QUEEN }); // Two Pair

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.ACE, Suit.DIAMONDS),
                new Card(Rank.KING, Suit.HEARTS),
                new Card(Rank.QUEEN, Suit.CLUBS),
                new Card(Rank.JACK, Suit.DIAMONDS),
                HandType.OnePair,
                new Rank[] { Rank.ACE, Rank.ACE, Rank.KING, Rank.QUEEN, Rank.JACK }); // One Pair

            TestHand(new Card(Rank.ACE, Suit.CLUBS),
                new Card(Rank.KING, Suit.HEARTS),
                new Card(Rank.QUEEN, Suit.CLUBS),
                new Card(Rank.JACK, Suit.DIAMONDS),
                new Card(Rank.NINE, Suit.DIAMONDS),
                HandType.HighCard,
                new Rank[] { Rank.ACE, Rank.KING, Rank.QUEEN, Rank.JACK, Rank.NINE }); // High Card
        }
    }
}
