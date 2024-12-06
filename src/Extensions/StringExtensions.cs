using System.Text;

namespace CT.Extensions;

public static class StringExtentions
{
    /// <summary>
    /// Paverčia įvestą tekstą į bitų masyvą.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static byte[] ConvertToBits(this string input)
    {
        var bytes = Encoding.ASCII.GetBytes(input);

        return bytes.ToBits();
    }

    /// <summary>
    /// Paverčia bitų masyvą į tekstą atsižvelgiant į naudojamą kodo dimensiją.
    /// </summary>
    /// <param name="input">Bitų masyvas</param>
    /// <param name="codeDimension">Kodo dimensija</param>
    /// <returns>Paverstas tekstas</returns>
    public static string ConvertFromBits(this List<byte[]> input, int size, int desiredSize)
    {
        var bytes = input.FromBits(size, desiredSize);

        return Encoding.ASCII.GetString([.. bytes]);
    }
}