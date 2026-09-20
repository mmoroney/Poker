namespace Combinatorics {
    public class BijectiveMapping {
        public static int Encode(int[] arr) {
            int code = 0;
            for (int i = 0; i < arr.Length; i++) {
                code += Choose(arr[i] + i, i + 1);
            }
            return code;
        }

        public static int[] Decode(int code, int length) {
            int[] arr = new int[length];
            for (int i = length - 1; i >= 0; i--) {
                int j = 0;
                while (Choose(j + i, i + 1) <= code) {
                    j++;
                }
                arr[i] = j - 1;
                code -= Choose(arr[i] + i, i + 1);
            }
            return arr;
        }

        private static int Choose(int n, int k) {
            if (k < 0 || k > n) return 0;
            if (k == 0 || k == n) return 1;
            k = Math.Min(k, n - k); // Take advantage of symmetry
            int c = 1;
            for (int i = 0; i < k; i++) {
                c = c * (n - i) / (i + 1);
            }
            return c;
        }   

    }
}
