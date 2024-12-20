using System.Drawing;
using CT.Extensions;
using CT.Models;

namespace CT.Services;

public static class InputService
{
    /// <summary>
    /// Užkoduoja, išsiunčia per kanalą ir dekoduoja vektorių.
    /// </summary>
    /// <param name="vector">Įvesties Vektorius</param>
    /// <param name="configuration">Programos konfiguracija</param>
    /// <param name="G">Generuojanti Matrica</param>
    /// <param name="H">Patikrinimo Matrica</param>
    public static void ProccessVector(byte[] vector, CodeParametersDto configuration, byte[][] G, byte[][] H)
    {
        var random = new Random();
        var input = new List<byte[]> { vector };

        {
            Console.WriteLine("Siuntimas Kanalu Su Kodavimu");
            var encoded = EncoderService.Vector(G, input);
            Console.WriteLine("\nUžkoduota įvestis:");
            encoded.Print();

            var transmitted = ChannelService.Transmit(random, configuration.ErrorRate, encoded);
            Console.WriteLine("\nIšsiųstą įvestis:");
            transmitted.Print();

            encoded.First().PrintDifferences(transmitted.First());

            Console.WriteLine("\nAr norite readaguoti iš kanalo išėjusį vektorių (t/n):");
            var willEdit = Console.ReadLine();
            Console.WriteLine();
            if(willEdit == "t")
            {
                var readVector = VectorExtensions.ReadVector(configuration.CodeLength);
                if(readVector == null)
                    return;

                transmitted = [readVector];
                Console.WriteLine();
            }

            var decoded = DecoderService.Vector(G, H, transmitted);
            Console.WriteLine("Dekoduota įvestis:");
            decoded.Print();
        }
        {
            Console.WriteLine("\nSiuntimas Kanalu Be Kodavimo");
            var transmitted = ChannelService.Transmit(random, configuration.ErrorRate, input);
            Console.WriteLine("\nGauta įvestis:");
            transmitted.Print();
        }
    }

    /// <summary>
    /// Užkoduoja, išsiunčia per kanalą ir dekoduoja tekstą išskaidytą į vektorių.
    /// </summary>
    /// <param name="vector">Įvesties Vektorius</param>
    /// <param name="configuration">Programos konfiguracija</param>
    /// <param name="G">Generuojanti Matrica</param>
    /// <param name="H">Patikrinimo Matrica</param>
    public static void ProccessText(byte[] vector, CodeParametersDto configuration, byte[][] G, byte[][] H)
    {
        var random = new Random();
        var input = vector.Prepare(8, configuration.CodeDimension);

        {    
            Console.WriteLine("Siuntimas Kanalu Su Kodavimu");

            var encoded = EncoderService.Vector(G, input);
            Console.WriteLine("\nUžkoduota įvestis:");
            Console.WriteLine(encoded.ConvertFromBits(8, configuration.CodeDimension));

            var transmitted = ChannelService.Transmit(random, configuration.ErrorRate, encoded);
            Console.WriteLine("\nIšsiųstą įvestis:");
            Console.WriteLine(transmitted.ConvertFromBits(8, configuration.CodeDimension));

            var decoded = DecoderService.Vector(G, H, transmitted);
            Console.WriteLine("\nDekoduota įvestis:");
            Console.WriteLine(decoded.ConvertFromBits(8, configuration.CodeDimension));
        }
        {
            Console.WriteLine("\nSiuntimas Kanalu Be Kodavimo");
            var transmitted = ChannelService.Transmit(random, configuration.ErrorRate, input);
            Console.WriteLine("\nGauta įvestis:");
            Console.WriteLine(transmitted.ConvertFromBits(8, configuration.CodeDimension));
        }    
    }

    /// <summary>
    /// Užkoduoja, išsiunčia per kanalą ir dekoduoja paveikslėlį išskaidytą į vektorių.
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="configuration"></param>
    /// <param name="G"></param>
    /// <param name="H"></param>
    /// <param name="name"></param>
    #pragma warning disable CA1416 // Validate platform compatibility
    public static void ProccessImage(string path, CodeParametersDto configuration, byte[][] G, byte[][] H)
    {
        var random = new Random();

        using var bitmap = new Bitmap(path);
        var input = bitmap.GetBytes().ToBits().Prepare(8, configuration.CodeDimension);

        {
            Console.WriteLine("Siuntimas Kanalu Su Kodavimu");

            var encoded = EncoderService.Vector(G, input);
            var transmitted = ChannelService.Transmit(random, configuration.ErrorRate, encoded);
            var decoded = DecoderService.Vector(G, H, transmitted);

            decoded.FromBits(8, configuration.CodeDimension).FromBytes(bitmap.Width, bitmap.Height).Save("decoded_" + path);
            Console.WriteLine("Įvestis išsaugota kaip decoded_" + path);
        }
        {
            Console.WriteLine("Siuntimas Kanalu Be Kodavimo");

            var transmitted = ChannelService.Transmit(random, configuration.ErrorRate, input);
            transmitted.FromBits(8, configuration.CodeDimension).FromBytes(bitmap.Width, bitmap.Height).Save("decoded_noenc_" + path);
            Console.WriteLine("Įvestis išsaugota kaip decoded_noenc_" + path);
        }
    }
    #pragma warning restore CA1416 // Validate platform compatibility

    /// <summary>
    /// Testuoja visus galimus vektorius tam tikroje kodo dimensijoje.
    /// </summary>
    /// <param name="configuration">Programos konfiguracija</param>
    /// <param name="G">Generuojanti Matrica</param>
    /// <param name="H">Patikrinimo Matrica</param>
    public static void ProccessTest(CodeParametersDto configuration, byte[][] G, byte[][] H)
    {
        var length = configuration.CodeDimension;
        var numCombinations = (int) Math.Pow(2, length);
        var combinations = new List<byte[]>();

        for (var i = 0; i < numCombinations; i++)
        {
            var combination = new byte[length];
            for (var j = 0; j < length; j++)
                combination[j] = (byte)((i >> j) & 1);
            combinations.Add(combination);
        }

        var random = new Random();
        foreach (var combo in combinations)
        {
            var input = new List<byte[]> { combo };
            input.Print();
            var encoded = EncoderService.Vector(G, input);
            encoded.Print();
            var transmitted = ChannelService.Transmit(random, configuration.ErrorRate, encoded);
            transmitted.Print();
            var decoded = DecoderService.Vector(G, H, transmitted);
            Console.WriteLine(decoded.First().Difference(combo));
            decoded.Print();

            if(!input.First().Compare(decoded.First()))
                Console.WriteLine("Klaida");

            Console.WriteLine();
        }
    }
}