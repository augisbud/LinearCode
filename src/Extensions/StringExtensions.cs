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
    public static string ConvertFromBits(this List<byte[]> input, int size, int desiredSize)
    {
        var vectorList = new List<byte[]>();

        foreach(var chunk in input)
        {
            if(desiredSize < size)
            {
                var useful = size % desiredSize;
                var vector = new byte[useful];
                Array.Copy(chunk, 0, vector, 0, useful);
                vectorList.Add(vector);
            }
            else
            {
                var vector = new byte[size];
                Array.Copy(chunk, 0, vector, 0, size);
                vectorList.Add(vector);
            }
        }

        var bits = vectorList.SelectMany(a => a).ToArray();

        var bytes = new byte[bits.Length / size];

        int index = 0;
        for (var i = 0; i < bits.Length; i += size)
        {
            byte b = 0;
            for (var j = 0; j < size; j++)
                b = (byte)((b << 1) + bits[i + j]);

            bytes[index++] = b;
        }

        return Encoding.ASCII.GetString([.. bytes]);
    }
}