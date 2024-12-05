using System.Text;
using CT;
using CT.Constants;
using CT.Extensions;
using CT.Models;
using CT.Services;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .Build()
    .GetSection("CodeParameters")
    .Get<CodeParametersDto>();

if (configuration == null)
{
    Console.WriteLine("Nepavyko nuskaityti konfigūracijos.");
    return;
}


var G = configuration.GeneratorMatrix ?? MatrixExtensions.Generator(configuration.CodeLength, configuration.CodeDimension);

if(!G.IsStandardForm())
{
    Console.WriteLine("Generuojanti matrica turi būti standartinės formos [Ik|P].");
    return;
}

var H = G.ParityMatrix();

Console.WriteLine("Generuojanti Matrica G:");
G.Print();
Console.WriteLine("\nKontrolinė Matrica H:");
H.Print();

Console.WriteLine("\n\nPasirinkite įvesties tipą (1 - vektorius, 2 - tekstas):");
var inputTypeString = Console.ReadLine();
if (!Enum.TryParse(inputTypeString, out InputType inputType))
{
    Console.WriteLine("Netinkamai pasirinktas žinutės tipas.");
    return;
}

switch (inputType)
{
    case InputType.Vector:
    {
        Console.WriteLine("Įveskite norimą vektorių:");
        var vectorString = Console.ReadLine();
        if (vectorString == null || vectorString == string.Empty || vectorString.Length != configuration.CodeDimension)
        {
            Console.WriteLine("Netinkamai įvestas vektorius.");
            return;
        }

        var vector = new byte[vectorString.Length];
        for (int i = 0; i < vectorString.Length; i++)
        {
            vector[i] = (byte) char.GetNumericValue(vectorString[i]);

            if(vector[i] != 0 && vector[i] != 1)
            {
                Console.WriteLine("Netinkamai įvestas vektorius.");
                return;
            }
        }

        VectorService.ProccessVector(vector, configuration, G, H);

        break;
    }
    case InputType.Text:
    {
        Console.WriteLine("Įveskite norimą tekstą:");
        var textString = Console.ReadLine();
        if (textString == null || textString == string.Empty)
        {
            Console.WriteLine("Netinkamai įvestas tekstas.");
            return;
        }

        var vector = textString.ConvertToBits();

        VectorService.ProccessText(vector, configuration, G, H);

        break;
    }
}