using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework;
using System;

namespace FrameworkTest {
    [TestClass]
    public class LowHandTest {
        [TestMethod]
        public void TestLowHandComparison() {
            for (int n = 0; n < 100; n++) {
                LowHand hand1 = Utilities.MakeLowHand();
                LowHand hand2 = Utilities.MakeLowHand();

                if (hand1 == hand2) {
                    Card[] cards1 = hand1.Cards();
                    Card[] cards2 = hand2.Cards();
                    for (int i = 0; i < 5; i++)
                        Assert.AreEqual(cards1[i].Rank, cards2[i].Rank);
                }
                else if (hand1 > hand2)
                    TestLowComparison(hand1, hand2);

                else
                    TestLowComparison(hand2, hand1);
            }
        }

        private static void TestLowComparison(LowHand stronger, LowHand weaker) {
            for (int i = 4; i >= 0; i--) {
                Card[] strongerCards = stronger.Cards();
                Card[] weakerCards = weaker.Cards();
                if (strongerCards[i].Rank == weakerCards[i].Rank)
                    continue;

                Assert.IsTrue(stronger.Cards()[i].Rank.LowComparable() < weaker.Cards()[i].Rank.LowComparable());
                return;
            }

            Assert.Fail();
        }
    }
}