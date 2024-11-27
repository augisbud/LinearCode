namespace CT;

// Current implementation does not support matrix's that are not in form of G = [Ik|P], need to implement Gaussian elimination
public static class Matrix
{
    private static readonly Random _random = new();

    public static byte[][] Generator(int n, int k)
    {
        var matrix = new byte[k][];

        for (int i = 0; i < k; i++)
        {
            matrix[i] = new byte[n];

            for (int j = 0; j < n; j++)
            {
                if (k - j > 0 && i == j)
                    matrix[i][j] = 1;
                else if (k - j > 0)
                    matrix[i][j] = 0;
                else
                    matrix[i][j] = (byte)_random.Next(0, 2);
            }
        }
        return matrix;
    }

    public static byte[][] ParityMatrix(byte[][] generatorMatrix)
    {
        int n = generatorMatrix[0].Length;
        int k = generatorMatrix.Length;

        byte[][] P = new byte[k][];
        for (int i = 0; i < k; i++)
        {
            P[i] = new byte[n - k];
            Array.Copy(generatorMatrix[i], k, P[i], 0, n - k);
        }

        byte[][] PT = new byte[n - k][];
        for (int i = 0; i < n - k; i++)
        {
            PT[i] = new byte[k];
            for (int j = 0; j < k; j++)
            {
                PT[i][j] = (byte)(P[j][i] == 0 ? 0 : 1);
            }
        }

        byte[][] I = new byte[n - k][];
        for (int i = 0; i < n - k; i++)
        {
            I[i] = new byte[n - k];
            I[i][i] = 1;
        }

        byte[][] H = new byte[n - k][];
        for (int i = 0; i < n - k; i++)
        {
            H[i] = new byte[n];

            for (int j = 0; j < k; j++)
            {
                H[i][j] = PT[i][j];
            }

            for (int j = 0; j < n - k; j++)
            {
                H[i][k + j] = I[i][j];
            }
        }

        return H;
    }

    public static void Print(byte[][] matrix)
    {
        foreach (var row in matrix)
        {
            foreach (var cell in row)
                Console.Write(cell + " ");
            Console.WriteLine();
        }
    }
}