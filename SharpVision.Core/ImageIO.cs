using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SharpVision.Core;

namespace SharpVision.Core;

public static class ImageIO
{
    public static async Task<Matrix<byte>> LoadGrayscaleAsync(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Could not find image at {filePath}");

        // L8: 8 bit luminance (gray)
        using Image<L8> image = await Image.LoadAsync<L8>(filePath);

        var matrix = new Matrix<byte>(image.Height, image.Width, 1);

        image.CopyPixelDataTo(matrix.Data);

        return matrix;
    }

    public static async Task<Matrix<byte>> LoadRgbAsync(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Could not find image at {filePath}");

        using Image<Rgb24> image = await Image.LoadAsync<Rgb24>(filePath);

        var matrix = new Matrix<byte>(image.Height, image.Width, 3);
        image.CopyPixelDataTo(matrix.Data);

        return matrix;
    }

    public static async Task SaveAsync(Matrix<byte> matrix, string filePath)
    {
        if (matrix.Channels == 1)
        {
            using Image<L8> image = Image.LoadPixelData<L8>(matrix.Data,
                                                            matrix.Columns,
                                                            matrix.Rows);
            await image.SaveAsync(filePath);
        }
        else if (matrix.Channels == 3)
        {
            using Image<Rgb24> image = Image.LoadPixelData<Rgb24>(matrix.Data,
                                                                  matrix.Columns,
                                                                  matrix.Rows);
            await image.SaveAsync(filePath);
        }
        else
        {
            throw new NotSupportedException($"Saving {matrix.Channels}-channel" +
                                            "images is not currently supported.");
        }
    }
}
