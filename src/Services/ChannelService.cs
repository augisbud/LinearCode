namespace CT.Services;

public static class ChannelService
{
    /// <summary>
    /// Siunčia vektorių per kanalą su klaido tikimybe p.
    /// </summary>
    /// <param name="random">Atsitiktinumo Generatorius</param>
    /// <param name="p">Klaidos Tikimybė p</param>
    /// <param name="vectors">Vektorius</param>
    /// <returns>Iš kanalo išėjęs vektorius</returns>
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