using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    public class HighLowHand {
        public HighHand HighHand {  get; private set; }
        public LowHand? LowHand { get; private set; }

        public HighLowHand(HighHand highHand, LowHand? lowHand) { 
            this.HighHand = highHand;
            this.LowHand = lowHand;
        }
    }
}
