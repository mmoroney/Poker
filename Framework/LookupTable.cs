using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework {
    public static class LookupTable {
        public static readonly ushort[] table;
        static LookupTable() {
            var assembly = typeof(LookupTable).Assembly;
            // default manifest resource name: <RootNamespace>.Data.five_card.dat
            string resourceName = assembly.GetName().Name + ".Data.five_card.dat";

            using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
            using MemoryStream ms = new();
            stream.CopyTo(ms);
            byte[] bytes = ms.ToArray();

            table = new ushort[bytes.Length / sizeof(ushort)];
            Buffer.BlockCopy(bytes, 0, table, 0, bytes.Length);
        }


        public static ushort Lookup(FiveCards cards) {
            return table[cards.Value];
        }
    }
}
