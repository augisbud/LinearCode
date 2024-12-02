using CT.Extensions;

namespace CT.Services;

public static class DecoderService
{
    public static List<byte[]> Vector(byte[][] G, byte[][] H, List<byte[]> input)
    {
        var result = new List<byte[]>();

        foreach(var vector in input)
        {
            result.Add(Vector(G, H, vector));
        }

        return result;
    }

    public static byte[] Vector(byte[][] G, byte[][] H, byte[] r)
    {
        var syndromes = CalculateSyndromes(G, H);

        for (var i = 0; i < r.Length; i++)
        {
            // (2) Compute H * r ^ T and the weight w of the corresponding coset leader.
            var w = syndromes[H.Transpose().Multiply(r).Normalize()];

            // (3) If w = 0, stop with r, as the transmitted codeword.
            if (w == 0)
                return r[..G.Length];

            // (4) Let e_i denote the binary n-tuple whose only non-zero component is i.
            var e_i = new byte[r.Length];
            e_i[i] = 1;

            // (5) If H * (r + e_i) ^ T has smaller associated weight (w_i) than H * r ^ T (w), set r = r + e_i.
            var r_i = r.Add(e_i);
            var w_i = syndromes[H.Transpose().Multiply(r_i).Normalize()];
            if (w_i < w)
                r = r_i;
        }

        return r[..G.Length];
    }

    private static Dictionary<string, int> CalculateSyndromes(byte[][] G, byte[][] H)
    {
        var n = G[0].Length;
        var r = H.Length;
        var totalSyndromes = 1 << r;
        var syndromeWeights = new Dictionary<string, int>();

        for (var weight = 0; weight <= n; weight++)
        {
            foreach (var indices in GetCombinations(n, weight))
            {
                var e = new byte[n];
                foreach (var idx in indices)
                    e[idx] = 1;

                var s = H.Transpose().Multiply(e);
                var sKey = s.Normalize();

                if (!syndromeWeights.TryGetValue(sKey, out int existingWeight) || existingWeight > weight)
                    syndromeWeights[sKey] = weight;

                if (syndromeWeights.Count >= totalSyndromes)
                    break;
            }
            
            if (syndromeWeights.Count >= totalSyndromes)
                break;
        }

        return syndromeWeights;
    }

    private static IEnumerable<int[]> GetCombinations(int n, int k)
    {
        if (k > n || k < 0)
            yield break;

        var result = new int[k];
        for (int i = 0; i < k; i++)
            result[i] = i;

        while (true)
        {
            yield return (int[])result.Clone();

            int i;
            for (i = k - 1; i >= 0; i--)
                if (result[i] != i + n - k)
                    break;

            if (i < 0)
                yield break;

            result[i]++;
            for (int j = i + 1; j < k; j++)
                result[j] = result[j - 1] + 1;
        }
    }
}