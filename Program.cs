using CT;
using CT.Extensions;
using CT.Services;

byte[] vector = [1, 0, 1];

var G = MatrixExtensions.Generator(6, vector.Length);
var H = G.ParityMatrix();

Console.WriteLine("Original:");
vector.Print();

Console.WriteLine("\nGenerator matrix G:");
G.Print();
Console.WriteLine("\nParity-check matrix H:");
H.Print();

var encoded = EncoderService.Vector(G, vector);
Console.WriteLine("\nEncoded:");
encoded.Print();

var transmitted = Channel.Transmit(0.2, encoded);
Console.WriteLine("\nTransmitted:");
transmitted.Print();

Console.WriteLine("\nDifference: " + encoded.Difference(transmitted));

var decoded = DecoderService.Vector(G, H, transmitted);
Console.WriteLine("\nDecoded:");
decoded.Print();

Console.WriteLine("\nDifference: " + vector.Difference(decoded));