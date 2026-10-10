using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Combinatorics;

namespace Framework.Games {
    internal static class Chooser {
        public static IEnumerable<Card[]> Choose(Card[] source, int count) {
            int n = BijectiveMapping.Choose(source.Length, count);
            for (int i = 0; i < n; i++) {
                yield return BijectiveMapping.Decode(i, count)
                    .Select(index => source[index])
                    .ToArray();
            }
        }
    }
}
