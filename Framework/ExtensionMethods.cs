using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    public static class ExtensionMethods {
        public static int LowComparable(this Rank rank) {
            int n = (int)rank;
            return n != (int)Rank.A ? n : -1;
        }

        public static bool IsLowRank(this Rank rank) {
            return rank.LowComparable() <= Rank._8.LowComparable();
        }
    }
}
