namespace CT;

public static class Channel
{
    public static byte[] Transmit(double p, byte[] vector)
    {
        var random = new Random();
        var transmitted = (byte[])vector.Clone();

        for (var i = 0; i < transmitted.Length; i++)
            if (random.NextDouble() < p)
                transmitted[i] ^= 1;

        return transmitted;
    }
}