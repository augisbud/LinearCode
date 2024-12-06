namespace CT.Extensions;

public static class ByteExtensions
{
    public static byte[] ToBits(this byte[] bytes)
    {
        byte[] bits = new byte[bytes.Length * 8];
        int index = 0;

        foreach (byte b in bytes)
            for (var i = 7; i >= 0; i--)
                bits[index++] = (byte)((b >> i) & 1);

        return bits;
    }

    public static byte[] FromBits(this List<byte[]> input, int size, int desiredSize)
    {
        var vectorList = new List<byte[]>();

        foreach(var chunk in input)
        {
            if(desiredSize < size)
            {
                var useful = size % desiredSize;
                if(useful == 0)
                    useful = desiredSize;
                    
                var vector = new byte[useful];
                Array.Copy(chunk, 0, vector, 0, useful);
                vectorList.Add(vector);
            }
            else
            {
                var vector = new byte[size];
                Array.Copy(chunk, 0, vector, 0, size);
                vectorList.Add(vector);
            }
        }

        var bits = vectorList.SelectMany(a => a).ToArray();

        var bytes = new byte[bits.Length / size];

        int index = 0;
        for (var i = 0; i < bits.Length; i += size)
        {
            byte b = 0;
            for (var j = 0; j < size; j++)
                b = (byte)((b << 1) + bits[i + j]);

            bytes[index++] = b;
        }

        return [.. bytes];
    }
}