namespace CT.Extensions;

public static class MatrixExtensions
{
    // Current implementation does not support matrix's that are not in form of G = [Ik|P]
    // To support that, we need to implement Gaussian elimination.
    private static readonly Random _random = new();

    /// <summary>
    /// Patikrina, ar matrica yra standartinės formos [Ik|P].
    /// </summary>
    /// <param name="matrix">Betkokia matrica</param>
    public static bool IsStandardForm(this byte[][] matrix)
    {
        for (var i = 0; i < matrix.Length; i++)
        {
            for (var j = 0; j < matrix.Length; j++)
            {
                if (i == j && matrix[i][j] != 1)
                    return false;
                if (i != j && matrix[i][j] != 0)
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Sugeneruoja atsitiktinę generuojančią matricą G.
    /// </summary>
    /// <param name="n">Kodo ilgis</param>
    /// <param name="k">Kodo dimensija</param>
    /// <returns></returns>
    public static byte[][] Generator(int n, int k)
    {
        var matrix = new byte[k][];

        for (var i = 0; i < k; i++)
        {
            matrix[i] = new byte[n];

            for (var j = 0; j < n; j++)
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

    /// <summary>
    /// Sugeneruoja patikrinimo matricą H iš generuojančios matricos G.
    /// </summary>
    /// <param name="generatorMatrix">Generuojanti Matrica</param>
    /// <returns></returns>
    public static byte[][] ParityMatrix(this byte[][] generatorMatrix)
    {
        int n = generatorMatrix[0].Length;
        int k = generatorMatrix.Length;

        byte[][] P = new byte[k][];
        for (var i = 0; i < k; i++)
        {
            P[i] = new byte[n - k];
            Array.Copy(generatorMatrix[i], k, P[i], 0, n - k);
        }

        byte[][] PT = new byte[n - k][];
        for (var i = 0; i < n - k; i++)
        {
            PT[i] = new byte[k];
            for (var j = 0; j < k; j++)
            {
                PT[i][j] = (byte)(P[j][i] == 0 ? 0 : 1);
            }
        }

        byte[][] I = new byte[n - k][];
        for (var i = 0; i < n - k; i++)
        {
            I[i] = new byte[n - k];
            I[i][i] = 1;
        }

        byte[][] H = new byte[n - k][];
        for (var i = 0; i < n - k; i++)
        {
            H[i] = new byte[n];

            for (var j = 0; j < k; j++)
            {
                H[i][j] = PT[i][j];
            }

            for (var j = 0; j < n - k; j++)
            {
                H[i][k + j] = I[i][j];
            }
        }

        return H;
    }

    /// <summary>
    /// Transponuoja matricą.
    /// </summary>
    /// <param name="matrix">Matrica Transponavimui</param>
    /// <returns></returns>
    public static byte[][] Transpose(this byte[][] matrix)
    {
        int rows = matrix.Length;
        int cols = matrix[0].Length;
        var transposed = new byte[cols][];

        for (var i = 0; i < cols; i++)
        {
            transposed[i] = new byte[rows];
            for (var j = 0; j < rows; j++)
            {
                transposed[i][j] = matrix[j][i];
            }
        }

        return transposed;
    }

    /// <summary>
    /// Daugina matricą iš vektoriaus.
    /// </summary>
    /// <param name="matrix">Matrica</param>
    /// <param name="vector">Vektorius</param>
    /// <returns>Matricos ir Vektoriaus daugybos rezultatas</returns>
    /// <exception cref="ArgumentException"></exception>
    public static byte[] Multiply(this byte[][] matrix, byte[] vector)
    {
        int rows = matrix.Length;
        int cols = matrix[0].Length;

        if (vector.Length != rows)
            throw new ArgumentException("Vektoriaus ilgis turi sutapti su eilučių skaičiumi matricoje.");

        var result = new byte[cols];

        for (var j = 0; j < cols; j++)
        {
            byte sum = 0;
            for (var i = 0; i < rows; i++)
                sum ^= (byte)(matrix[i][j] * vector[i]);

            result[j] = (byte)(sum % 2);
        }

        return result;
    }

    /// <summary>
    /// Atspausdina matricą į konsolę.
    /// </summary>
    /// <param name="matrix">Matrica, kurią norima atspausdinti</param>
    public static void Print(this byte[][] matrix)
    {
        foreach (var row in matrix)
        {
            foreach (var cell in row)
                Console.Write(cell + " ");
            Console.WriteLine();
        }
    }
}