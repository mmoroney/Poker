using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Games {
    internal static class Chooser {
        public static IEnumerable<Card[]> Choose(Card[] source, Card[] dest) {
            return Choose(source, 0, dest, 0, dest.Length);
        }

        public static IEnumerable<Card[]> Choose(Card[] source, int sourceIndex, Card[] dest, int destIndex, int count) {
            if (count == 0) {
                yield return dest;
                yield break;
            }

            for (int i = sourceIndex; i < source.Length; i++) {
                dest[destIndex] = source[i];
                foreach (Card[] result in Choose(source, i + 1, dest, destIndex + 1, count - 1))
                    yield return result;
            }
        }
    }
}
