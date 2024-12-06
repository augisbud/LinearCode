using System.Text;
using CT.Models;

namespace CT.Extensions;

public static class VectorExtensions
{
    /// <summary>
    /// Nuskaito vektorių iš įvesties.
    /// </summary>
    /// <param name="codeDimension">Kodo Dimensija</param>
    /// <returns></returns>
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
        for (var i = 0; i < vectorString.Length; i++)
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

    /// <summary>
    /// Paruošia vektorių siuntimui.
    /// </summary>
    /// <param name="vector">Vektorius</param>
    /// <param name="size">Skaidymo dydis</param>
    /// <returns></returns>
    public static List<byte[]> Prepare(this byte[] vector, int size, int desiredSize)
    {
        var individual = new List<byte[]>();

        for (var i = 0; i < vector.Length; i += size)
        {
            var chunk = new byte[size];
            Array.Copy(vector, i, chunk, 0, Math.Min(size, vector.Length - i));
            individual.Add(chunk);
        }

        var prepared = new List<byte[]>();

        foreach(var chunk in individual)
        {
            if (desiredSize < size)
            {
                var useful = size % desiredSize;
                if(useful == 0)
                    useful = desiredSize;

                for(var i = 0; i < chunk.Length; i++)
                {
                    var newVector = new byte[desiredSize];
                    Array.Copy(chunk, i, newVector, 0, useful);
                    prepared.Add(newVector);

                    i += useful - 1;
                }
            }
            else
            {
                var newVector = new byte[desiredSize];
                Array.Copy(chunk, 0, newVector, 0, chunk.Length);
                prepared.Add(newVector);
            }
        }

        return prepared;
    }

    /// <summary>
    /// Palygina du vektorius ir grąžina jų skirtumą.
    /// </summary>
    /// <param name="input">Pirmasis Vektorius</param>
    /// <param name="other">Antrasis Vektorius</param>
    /// <returns></returns>
    public static int Difference(this byte[] input, byte[] other)
    {
        var differences = 0;

        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] != other[i])
                differences++;
        }

        return differences;
    }

    /// <summary>
    /// Palygina du vektorius ir grąžina ar jie sutampa.
    /// </summary>
    /// <param name="input">Pirmasis Vektorius</param>
    /// <param name="other">Antrasis Vektorius</param>
    /// <returns></returns>
    public static bool Compare(this byte[] input, byte[] other)
    {
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] != other[i])
                return false;
        }

        return true;
    }

    /// <summary>
    /// Atspausdina skirtumus tarp dviejų vektorių.
    /// </summary>
    /// <param name="input">Pirmasis Vektorius</param>
    /// <param name="other">Antrasis Vektorius</param>
    public static void PrintDifferences(this byte[] input, byte[] other)
    {
        var differences = 0;
        var maxLength = Math.Max(input.Length, other.Length);
        var sb = new StringBuilder();

        for (var i = 0; i < maxLength; i++)
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

    /// <summary>
    /// Atspausdina vektorių į konsolę.
    /// </summary>
    /// <param name="input">Vektorius atspausdinimui</param>
    public static void Print(this List<byte[]> input)
    {
        foreach (var vector in input)
        {
            foreach (var cell in vector)
                Console.Write(cell);
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Sudeda du vektorius.
    /// </summary>
    /// <param name="vector1">Pirmasis Vektorius</param>
    /// <param name="vector2">Antrasis Vektorius</param>
    /// <returns>Vektorių Sudėties Rezultatas</returns>
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

    /// <summary>
    /// Paverčia vektorių į tekstą.
    /// </summary>
    /// <param name="vector">Vektorius</param>
    /// <returns>Paverstas tekstas</returns>
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