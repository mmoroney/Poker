using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    public static class ExtensionMethods {
        public static int LowComparable(this Rank rank) {
            return rank.Value == 12 ? -1 : rank.Value;
        }

        public static bool IsLowRank(this Rank rank) {
            return rank.Value < 7 || rank.Value == 12;
        }
    }
}
