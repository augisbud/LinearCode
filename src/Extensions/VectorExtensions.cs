using System.Text;
using CT.Models;

namespace CT.Extensions;

public static class VectorExtensions
{
    public static byte[]? ReadVector(int codeDimension)
    {
        Console.WriteLine("Įveskite norimą vektorių:");
        var vectorString = Console.ReadLine();
        if (vectorString == null || vectorString == string.Empty || vectorString.Length != codeDimension)
        {
            Console.WriteLine("Netinkamai įvestas vektorius.");
            Console.ReadLine();
            return null;
        }

        var vector = new byte[vectorString.Length];
        for (int i = 0; i < vectorString.Length; i++)
        {
            vector[i] = (byte) char.GetNumericValue(vectorString[i]);

            if(vector[i] != 0 && vector[i] != 1)
            {
                Console.WriteLine("Netinkamai įvestas vektorius.");
                Console.ReadLine();
                return null;
            }
        }

        return vector;
    }
    public static List<byte[]> Prepare(this byte[] vector, int size)
    {
        var prepared = new List<byte[]>();

        for (var i = 0; i < vector.Length; i += size)
        {
            var chunk = new byte[size];
            Array.Copy(vector, i, chunk, 0, size);

            prepared.Add(chunk);
        }

        return prepared;
    }

    public static void PrintDifferences(this byte[] input, byte[] other)
    {
        var differences = 0;
        var maxLength = Math.Max(input.Length, other.Length);
        var sb = new StringBuilder();

        for (int i = 0; i < maxLength; i++)
        {
            if (input[i] != other[i])
            {
                differences++;
                sb.Append($"[{input[i]}->{other[i]}]"); 
            }
            else
                sb.Append($"{input[i]}");
        }

        Console.WriteLine("\nIšsiųstos ir gautos įvestys skiriasi {0} vietose.", differences);
        Console.WriteLine("Jos pažymėtos: {0}", sb.ToString());
    }

    public static void Print(this List<byte[]> input)
    {
        foreach (var vector in input)
        {
            foreach (var cell in vector)
                Console.Write(cell + " ");
            Console.WriteLine();
        }
    }
    
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
}