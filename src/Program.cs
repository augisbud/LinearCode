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
    Console.WriteLine("Nepavyko nuskaityti konfigūracijos. Ar turite appsettings.json failą su 'CodeParameters' sekciją kaip nurodyta README.md?");
    Console.ReadLine();
    return;
}

var G = configuration.GeneratorMatrix ?? MatrixExtensions.Generator(configuration.CodeLength, configuration.CodeDimension);

if(!G.IsStandardForm())
{
    Console.WriteLine("Generuojanti matrica turi būti standartinės formos [Ik|P].");
    Console.ReadLine();
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
    Console.ReadLine();
    return;
}

switch (inputType)
{
    case InputType.Vector:
    {
        var vector = VectorExtensions.ReadVector(configuration.CodeDimension);
        if(vector == null)
            return;

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
            Console.ReadLine();
            return;
        }

        var vector = textString.ConvertToBits();

        VectorService.ProccessText(vector, configuration, G, H);

        break;
    }
}

Console.ReadLine();