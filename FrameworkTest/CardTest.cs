using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework;
using System.Collections.Generic;

namespace FrameworkTest {
    [TestClass]
    public class CardTest {
        [TestMethod]
        public void TestEquality() {
            int ranks = Enum.GetValues(typeof(Rank)).Length;
            int suits = Enum.GetValues(typeof(Suit)).Length;

            for (int i = 0; i < ranks * suits; i++) {
                for (int j = 0; j < ranks * suits; j++) {
                    Card card1 = new(i);
                    Card card2 = new(j);
                    bool expected = i == j;
                    Assert.AreEqual(expected, card1.Equals(card2));
                    Assert.AreEqual(expected, card1 == card2);
                    Assert.IsFalse(card1.Equals(null));
                }
            }
        }

        [TestMethod]
        public void TestHashCode() {
            int ranks = Enum.GetValues(typeof(Rank)).Length;
            int suits = Enum.GetValues(typeof(Suit)).Length;

            HashSet<Card> set = new();

            for (int i = 0; i < ranks * suits; i++) {
                Assert.AreEqual(i, set.Count);
                Card card = new(i);
                Assert.IsFalse(set.Contains(card));
                set.Add(card);
                Assert.AreEqual(i + 1, set.Count);
                Assert.IsTrue(set.Contains(new Card(i)));
            }
        }
    }
}