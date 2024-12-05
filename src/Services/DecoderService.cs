using CT.Extensions;

namespace CT.Services;

public static class DecoderService
{
    /// <summary>
    /// Dekoduoja vektorių sąrašą
    /// </summary>
    /// <param name="G">Generuojanti Matrica</param>
    /// <param name="H">Patikrinimo Matrica</param>
    /// <param name="input">Iš kanalo gautas vektorius</param>
    /// <returns>Dekoduotas vektorius</returns>
    public static List<byte[]> Vector(byte[][] G, byte[][] H, List<byte[]> input)
    {
        var result = new List<byte[]>();

        foreach(var vector in input)
        {
            result.Add(Vector(G, H, vector));
        }

        return result;
    }

    /// <summary>
    /// Dekoduoja vektorių
    /// </summary>
    /// <param name="G">Generuojanti Matrica</param>
    /// <param name="H">Patikrinimo Matrica</param>
    /// <param name="r">Iš kanalo gautas vektorius</param>
    /// <returns>Dekoduotas vektorius</returns>
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

    /// <summary>
    /// Sudaro sindromų ir jų svorių lentelę
    /// </summary>
    /// <param name="G">Generuojanti Matrica</param>
    /// <param name="H">Patikrinimo Matrica</param>
    /// <returns></returns>
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
                // Sukuriame klaidų vektorių e, įrašydami 1 į nurodytas pozicijas
                var e = new byte[n];
                foreach (var idx in indices)
                    e[idx] = 1;

                // Naudojame matricą H, kad apskaičiuotume sindromą
                var s = H.Transpose().Multiply(e);
                var sKey = s.Normalize();

                // Išsaugome sindromo svorį, jei jo neturime, arba jis mažesnis
                if (!syndromeWeights.TryGetValue(sKey, out int existingWeight) || existingWeight > weight)
                    syndromeWeights[sKey] = weight;

                // Baigiame darbą, kai surašytos visos galimos kombinacijos
                if (syndromeWeights.Count >= totalSyndromes)
                    break;
            }
            
            if (syndromeWeights.Count >= totalSyndromes)
                break;
        }

        return syndromeWeights;
    }

    // Sugeneruojame visus įmanomus klaidų kombinacijas (k klaidų, n ilgio vektoriams)
    private static IEnumerable<int[]> GetCombinations(int n, int k)
    {
        if (k > n || k < 0)
            yield break;

        var result = new int[k];
        for (var i = 0; i < k; i++)
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
            for (var j = i + 1; j < k; j++)
                result[j] = result[j - 1] + 1;
        }
    }
}