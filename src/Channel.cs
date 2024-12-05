namespace CT;

public static class Channel
{
    public static List<byte[]> Transmit(Random random, double p, List<byte[]> vectors)
    {
        var transmittedVectors = new List<byte[]>();

        foreach (var vector in vectors)
        {
            var transmitted = (byte[])vector.Clone();

            for (var i = 0; i < transmitted.Length; i++)
                if (random.NextDouble() < p)
                    transmitted[i] ^= 1;

            transmittedVectors.Add(transmitted);
        }

        return transmittedVectors;
    }
}