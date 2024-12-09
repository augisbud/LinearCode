using System.Drawing;

namespace CT.Extensions;

#pragma warning disable CA1416 // Validate platform compatibility
public static class BitmapExtensions
{
    /// <summary>
    /// Paverčia paveikslėlį į baitų masyvą.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static byte[] GetBytes(this Bitmap input)
    {
        var bytes = new byte[input.Width * input.Height * 3];
        var index = 0;

        for (var y = 0; y < input.Height; y++)
        {
            for (var x = 0; x < input.Width; x++)
            {
                var pixel = input.GetPixel(x, y);
                bytes[index++] = pixel.R;
                bytes[index++] = pixel.G;
                bytes[index++] = pixel.B;
            }
        }

        return bytes;
    }

    /// <summary>
    /// Paverčia baitų masyvą į paveikslėlį.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static Bitmap FromBytes(this byte[] input, int width, int height)
    {
        var bitmap = new Bitmap(width, height);

        var index = 0;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var r = input[index++];
                var g = input[index++];
                var b = input[index++];
                bitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
            }
        }

        return bitmap;
    }
}
#pragma warning restore CA1416 // Validate platform compatibility