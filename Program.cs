using CT;

byte[] vector = [1, 0, 1, 1];

var G = Matrix.Generator(7, vector.Length);
var H = Matrix.ParityMatrix(G);