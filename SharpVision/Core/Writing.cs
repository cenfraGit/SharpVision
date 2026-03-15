using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SharpVision;

public static partial class Sharp
{
    public static void SaveImage(Matrix<byte> src, string dst)
    {
        if (src.Channels == 1)
        {
            using var image = Image.LoadPixelData<L8>(src.Data, src.Columns, src.Rows);
            image.Save(dst);
        }
        else if (src.Channels == 3)
        {
            using var image = Image.LoadPixelData<Rgb24>(src.Data, src.Columns, src.Rows);
            image.Save(dst);
        }
        else
        {
            throw new NotSupportedException($"Saving images with {src.Channels} channels is not supported.");
        }
    }
}
