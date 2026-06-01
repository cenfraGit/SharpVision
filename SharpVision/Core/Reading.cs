using System;
using System.IO;
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
    [SharpFunction("Read/Write", "Reads an image from a path.")]
    public static void ReadImage([SharpOutput] Matrix<byte> dst, string src, ReadMode readMode = ReadMode.Default)
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
            dst.ReallocateIfNeeded(image.Height, image.Width, 1);
            image.CopyPixelDataTo(dst.Data);
        }
        else
        {
            using var image = Image.Load<Rgb24>(src);
            dst.ReallocateIfNeeded(image.Height, image.Width, 3);
            image.CopyPixelDataTo(dst.Data);
        }
    }
}
