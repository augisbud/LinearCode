using CT;

byte[] vector = [1, 0, 1, 1];

var G = Matrix.Generator(7, vector.Length);
var H = Matrix.ParityMatrix(G);

var encoded = Encoder.Vector(G, vector);
Helpers.Print(encoded);

var transmitted = Channel.Transmit(0.1, encoded);
Helpers.Print(transmitted);