using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework;
using System;
using Framework.Games;
using System.Collections.Generic;

namespace FrameworkTest {
    [TestClass]
    public class HoldemTest {
        [TestMethod]
        public void TestCalculateEquity() {
            var testData = new[] {
                new {  Board = "QcTc3h",  HoleCards = new string[] { "AcKc", "QhQd" },  Equities = new Rational[] {new Rational(67, 198), new Rational(131,198) } },
                new {  Board = "QcTc3h5d",  HoleCards = new string[] { "AcKc", "QhQd" },  Equities = new Rational[] {new Rational(10, 44), new Rational(34, 44) } },
                new {  Board = "AsAcAhAdKs",  HoleCards = new string[] { "TsTh", "KcKd", "2h2s" },  Equities = new Rational[] {new Rational(1, 3), new Rational(1, 3), new Rational(1, 3) } },
            };

            foreach (var data in testData)
                TestCalculateEquity(data.Board, data.HoleCards, data.Equities);
        }

        private static void TestCalculateEquity(string board, string[] holeCards, Rational[] equities) {
            Rational[] results = Holdem.CalculateEquity(board, holeCards);
            Assert.AreEqual(holeCards.Length, results.Length);

            for (int i = 0; i < holeCards.Length; i++)
                Assert.AreEqual(equities[i], results[i]);
        }
    }
}