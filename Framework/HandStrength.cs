using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    [DebuggerDisplay("{ToString()}")]
    public readonly record struct HandStrength(int Value) {
        public HandType HandType => new((byte)(Value >> 20));
        public Rank[] Ranks {
            get {
                Rank[] ranks = new Rank[5];
                for (int i = 0; i < 5; i++) {
                    ranks[4 -i] = new((Value >> (4 * i)) & 0xF);
                }
                return ranks;
            }
        }
        public override string ToString() {
            return $"{string.Join("", Ranks.Select(r => r.ToString()))}{(IsSuited ? "s" : "")} ({HandType})";
        }

        private bool IsSuited => HandType == HandType.Flush || HandType == HandType.StraightFlush;
    }
}
