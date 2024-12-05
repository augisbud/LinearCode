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

        byte[] bits = new byte[bytes.Length * 8];
        int index = 0;

        foreach (byte b in bytes)
            for (var i = 7; i >= 0; i--)
                bits[index++] = (byte)((b >> i) & 1);

        return bits;
    }

    /// <summary>
    /// Paverčia bitų masyvą į tekstą atsižvelgiant į naudojamą kodo dimensiją.
    /// </summary>
    /// <param name="input">Bitų masyvas</param>
    /// <param name="codeDimension">Kodo dimensija</param>
    /// <returns>Paverstas tekstas</returns>
    public static string ConvertFromBits(this List<byte[]> input, int codeDimension)
    {
        var bytes = new List<byte>();

        for (var i = 0; i < input.Count; i += codeDimension)
        {
            byte b = 0;
            int bitIndex = 0;

            for (var j = 0; j < codeDimension; j++)
            {
                if (i + j >= input.Count)
                    break;

                var bitArray = input[i + j];

                for (var k = 0; k < codeDimension; k++)
                {
                    b = (byte)(b << 1 | bitArray[k]);
                    bitIndex++;

                    if (bitIndex == 8)
                    {
                        bytes.Add(b);
                        b = 0;
                        bitIndex = 0;
                    }
                }
            }

            if (bitIndex > 0)
            {
                b <<= (8 - bitIndex);
                bytes.Add(b);
            }
        }

        return Encoding.ASCII.GetString(bytes.ToArray());
    }
}