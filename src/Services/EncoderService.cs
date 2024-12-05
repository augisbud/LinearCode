using CT.Extensions;

namespace CT.Services;

public static class EncoderService
{
    /// <summary>
    /// Užkoduoja duotą vektorių daugindamas jį iš generuojančios matricos.
    /// </summary>
    /// <param name="matrix">Matrica</param>
    /// <param name="input">Vektorius Užkodavimui</param>
    /// <returns>Užkoduotas vektorius</returns>
    public static List<byte[]> Vector(byte[][] matrix, List<byte[]> input)
    {
        var encoded = new List<byte[]>();

        foreach(var vector in input)
        {
            encoded.Add(matrix.Multiply(vector));
        }
        
        return encoded;
    }
}