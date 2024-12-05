using CT.Extensions;
using CT.Models;

namespace CT.Services;

public static class VectorService
{
    public static void ProccessVector(byte[] vector, CodeParametersDto configuration, byte[][] G, byte[][] H)
    {
        var input = vector.Prepare(configuration.CodeDimension);
        Console.WriteLine("Originali, išskaidytą įvestis:");
        input.Print();

        var encoded = EncoderService.Vector(G, input);
        Console.WriteLine("\nUžkoduota įvestis:");
        encoded.Print();

        var random = new Random();
        var transmitted = Channel.Transmit(random, configuration.ErrorRate, encoded);
        Console.WriteLine("\nIšsiųstą įvestis:");
        transmitted.Print();

        // TODO: show bytes that differ.
        Console.WriteLine("\nIšsiųstos ir gautos įvestys skiriasi {0} vietose.", encoded.Difference(transmitted));

        var decoded = DecoderService.Vector(G, H, transmitted);
        Console.WriteLine("\nDekoduota įvestis:");
        decoded.Print();
    }

    public static void ProccessText(byte[] vector, CodeParametersDto configuration, byte[][] G, byte[][] H)
    {
        var input = vector.Prepare(configuration.CodeDimension);
        Console.WriteLine("Originali, išskaidytą įvestis:");
        Console.WriteLine(input.ConvertFromBits(configuration.CodeDimension));

        var encoded = EncoderService.Vector(G, input);
        Console.WriteLine("\nUžkoduota įvestis:");
        Console.WriteLine(encoded.ConvertFromBits(configuration.CodeDimension));

        var random = new Random();
        var transmitted = Channel.Transmit(random, configuration.ErrorRate, encoded);
        Console.WriteLine("\nIšsiųstą įvestis:");
        Console.WriteLine(transmitted.ConvertFromBits(configuration.CodeDimension));

        var rawDecoded = DecoderService.Vector(G, H, encoded);
        Console.WriteLine("\nIškart dekoduota įvestis:");
        Console.WriteLine(rawDecoded.ConvertFromBits(configuration.CodeDimension));

        var decoded = DecoderService.Vector(G, H, transmitted);
        Console.WriteLine("\nDekoduota įvestis:");
        Console.WriteLine(decoded.ConvertFromBits(configuration.CodeDimension));
    }
}