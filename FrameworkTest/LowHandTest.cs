using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework;
using System;

namespace FrameworkTest {
    [TestClass]
    public class LowHandTest {
        [TestMethod]
        public void TestLowHandComparison() {
            for (int n = 0; n < 100; n++) {
                byte hand1 = Utilities.MakeLowHand();
                byte hand2 = Utilities.MakeLowHand();
                Rank[] ranks1 = Framework.Decoder.DecodeLow(hand1);
                Rank[] ranks2 = Framework.Decoder.DecodeLow(hand2);

                if (hand1 == hand2) {
                    for (int i = 0; i < 5; i++)
                        Assert.AreEqual(ranks1[i], ranks2[i]);
                }
                else if (hand1 > hand2)
                    TestLowComparison(ranks2, ranks1);

                else
                    TestLowComparison(ranks1, ranks2);
            }
        }

        private static void TestLowComparison(Rank[] stronger, Rank[] weaker) {
            for (int i = 4; i >= 0; i--) {
                if (stronger[i] == weaker[i])
                    continue;

                Assert.IsTrue(stronger[i].LowComparable() < weaker[i].LowComparable());
                return;
            }

            Assert.Fail();
        }
    }
}