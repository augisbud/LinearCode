using System.Text;

namespace CT.Extensions;

public static class VectorExtensions
{
    public static byte[] Add(this byte[] vector1, byte[] vector2)
    {
        if (vector1.Length != vector2.Length)
            throw new ArgumentException("Vektorių ilgis turi sutapti.");

        var result = new byte[vector1.Length];

        for (var i = 0; i < vector1.Length; i++)
        {
            result[i] = (byte)(vector1[i] ^ vector2[i]);
        }

        return result;
    }

    public static string Normalize(this byte[] vector)
    {
        if (vector == null || vector.Length == 0)
            return string.Empty;

        var stringBuilder = new StringBuilder();
        foreach (var byteValue in vector)
        {
            stringBuilder.Append(byteValue);
        }

        return stringBuilder.ToString();
    }

    public static void Print(this byte[] vector)
    {
        foreach (var cell in vector)
            Console.Write(cell + " ");
        Console.WriteLine();
    }

    public static int Difference(this byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
            throw new ArgumentException("Vektorių ilgiai turi sutapti.");

        int count = 0;

        for (int i = 0; i < a.Length; i++)
            if (a[i] != b[i])
                count++;

        return count;
    }
}