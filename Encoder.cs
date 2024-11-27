namespace CT;

public static class Encoder
{
    public static byte[] Vector(byte[][] matrix, byte[] vector)
    {
        int rows = matrix.Length;
        int cols = matrix[0].Length;

        if (vector.Length != rows)
            throw new ArgumentException("Vektoriaus ilgis turi sutapti su eilučių skaičiumi matricoje.");

        var result = new byte[cols];

        for (int j = 0; j < cols; j++)
        {
            byte sum = 0;
            for (int i = 0; i < rows; i++)
                sum ^= (byte) (matrix[i][j] * vector[i]);
                
            result[j] = (byte) (sum % 2);
        }

        return result;
    }
}