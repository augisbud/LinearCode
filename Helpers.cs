namespace CT;

public static class Helpers
{
    public static void Print(byte[][] matrix)
    {
        foreach (var row in matrix)
        {
            foreach (var cell in row)
                Console.Write(cell + " ");
            Console.WriteLine();
        }
    }

    public static void Print(byte[] vector)
    {
        foreach (var cell in vector)
            Console.Write(cell + " ");
        Console.WriteLine();
    }
}