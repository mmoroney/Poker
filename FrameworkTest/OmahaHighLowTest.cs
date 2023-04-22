﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework;
using System;
using Framework.Games;

namespace FrameworkTest {
    [TestClass]
    public class OmahaHighLowTest {
        [TestMethod]
        public void TestCalculateEquity() {
            var testData = new[] {
                new {  Board = "KcQcTh4d",  HoleCards = new string[] {"AcAsJcTs", "KdKh7d2s"},  Equities = new Rational[] {new Rational(32, 40), new Rational(8, 40) } },
                new {  Board = "AcTs9h",  HoleCards = new string[] { "KcQdJh8s", "AsAh2h3d"},  Equities = new Rational[] {new Rational(647, 1640), new Rational(993, 1640) } },
            };

            foreach (var data in testData)
                TestCalculateEquity(data.Board, data.HoleCards, data.Equities);
        }

        private static void TestCalculateEquity(string board, string[] holeCards, Rational[] equities) {
            Rational[] results = OmahaHighLow.CalculateEquity(board, holeCards);
            Assert.AreEqual(holeCards.Length, results.Length);

            for (int i = 0; i < holeCards.Length; i++)
                Assert.AreEqual(equities[i], results[i]);
        }
    }
}