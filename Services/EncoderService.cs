using CT.Extensions;

namespace CT.Services;

public static class EncoderService
{
    public static byte[] Vector(byte[][] matrix, byte[] vector)
    {
        return matrix.Multiply(vector);
    }
}