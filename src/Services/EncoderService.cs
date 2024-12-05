using CT.Extensions;

namespace CT.Services;

public static class EncoderService
{
    public static List<byte[]> Vector(byte[][] matrix, List<byte[]> input)
    {
        var encoded = new List<byte[]>();

        foreach(var vector in input)
        {
            encoded.Add(matrix.Multiply(vector));
        }
        
        return encoded;
    }
}