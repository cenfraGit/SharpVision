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
    Default = 0,
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
    private static object[] ReadImage2(string src, string readMode)
    {
        ReadMode mode;
        if (readMode == "default")
            mode = ReadMode.Default;
        else if (readMode == "grayscale")
            mode = ReadMode.Grayscale;
        else if (readMode == "color")
            mode = ReadMode.Color;
        else
            throw new ArgumentException($"Invalid readMode value: {readMode}.");

        Matrix<byte> dst = ReadImage(src, mode);
        return new object[] { dst };
    }
}
