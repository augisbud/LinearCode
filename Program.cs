using CT;
using CT.Extensions;
using CT.Services;

byte[] vector = [1, 1, 0, 0];

var G = MatrixExtensions.Generator(7, 4);
var H = G.ParityMatrix();

var input = vector.Prepare(G.Length);

Console.WriteLine("Original:");
input.Print();

Console.WriteLine("\nGenerator matrix G:");
G.Print();
Console.WriteLine("\nParity-check matrix H:");
H.Print();

var encoded = EncoderService.Vector(G, input);
Console.WriteLine("\nEncoded:");
encoded.Print();

var transmitted = Channel.Transmit(0.1, encoded);
Console.WriteLine("\nTransmitted:");
transmitted.Print();

var decoded = DecoderService.Vector(G, H, transmitted);

Console.WriteLine("\nDecoded:");
decoded.Print();