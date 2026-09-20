using System.Collections.Generic;
using Xunit;

namespace Combinatorics.Tests
{
    public class BijectiveMappingTests
    {
        // Hoist reused arrays to static readonly fields to avoid repeated allocations.
        private static readonly int[] V_2_3 = [ 2, 3 ];
        private static readonly int[] V_3_5 = [ 3, 5 ];
        private static readonly int[] V_0_4 = [ 0, 4 ];
        private static readonly int[] V_4_5 = [ 4, 5 ];
        private static readonly int[] V_5_7 = [ 5, 7 ];

        private static readonly int[] L3_A = [ 3, 4, 5 ];
        private static readonly int[] L3_B = [ 0, 2, 6 ];
        private static readonly int[] L3_C = [ 4, 5, 7 ];

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(42)]
        public void Encode_SingleElement_ReturnsValueAndDecodeRoundTrips(int value)
        {
            var arr = new int[] { value };
            int code = BijectiveMapping.Encode(arr);
            Assert.Equal(value, code);
            var decoded = BijectiveMapping.Decode(code, arr.Length);
            Assert.Equal(arr, decoded);
        }

        public static IEnumerable<object[]> Vectors()
        {
            yield return new object[] { V_2_3 };
            yield return new object[] { V_3_5 };
            yield return new object[] { V_0_4 };
            yield return new object[] { V_4_5 };
            yield return new object[] { V_5_7 };
        }

        [Theory]
        [MemberData(nameof(Vectors))]
        public void EncodeDecode_Roundtrip_ForLength2Vectors(int[] arr)
        {
            int code = BijectiveMapping.Encode(arr);
            var decoded = BijectiveMapping.Decode(code, arr.Length);
            Assert.Equal(arr, decoded);
        }

        [Fact]
        public void EncodeDecode_Roundtrip_SomeLength3Vectors()
        {
            var list = new List<int[]>
            {
                L3_A,
                L3_B,
                L3_C
            };
            foreach (var arr in list)
            {
                int code = BijectiveMapping.Encode(arr);
                var decoded = BijectiveMapping.Decode(code, arr.Length);
                Assert.Equal(arr, decoded);
            }
        }
    }
}
