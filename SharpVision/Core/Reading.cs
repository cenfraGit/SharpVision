using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SharpVision;

// --------------------------------------------------------------------------------
// readmode
// --------------------------------------------------------------------------------

public enum ReadMode
{
    Default,
    Grayscale,
    Color
}

// --------------------------------------------------------------------------------
// read image
// --------------------------------------------------------------------------------

public static partial class Sharp
{
    public static Matrix<byte> ReadImage(string src, ReadMode readMode = ReadMode.Default)
    {
        if (!File.Exists(src))
            throw new FileNotFoundException($"Could not find image at {src}");

        if (readMode == ReadMode.Default)
        {
            var info = Image.Identify(src);
            readMode = (info.PixelType.BitsPerPixel == 8) ? ReadMode.Grayscale : ReadMode.Color;
        }

        if (readMode == ReadMode.Grayscale)
        {
            using var image = Image.Load<L8>(src);
            var matrix = new Matrix<byte>(image.Height, image.Width, 1);
            image.CopyPixelDataTo(matrix.Data);
            return matrix;
        }
        else
        {
            using var image = Image.Load<Rgb24>(src);
            var matrix = new Matrix<byte>(image.Height, image.Width, 3);
            image.CopyPixelDataTo(matrix.Data);
            return matrix;
        }
    }

    [SharpFunction("Read/Write", "Reads an image from a path.")]
    private static object[] ReadImage(string src)
    {
        Matrix<byte> dst = ReadImage(src, ReadMode.Default);
        return new object[] { dst };
    }
}
