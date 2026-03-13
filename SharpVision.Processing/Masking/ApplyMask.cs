using System;
using SharpVision.Core;

namespace SharpVision.Processing;

public static partial class Masking
{
    public static void ApplyMask(Matrix<byte> image, Matrix<byte> mask)
    {
        if (image.Rows != mask.Rows || image.Columns != mask.Columns)
            throw new ArgumentException("Image and mask must have same dimensions.");
            
        if (mask.Channels != 1)
            throw new ArgumentException("Mask must be a 1-channel grayscale image.");

        for (int row = 0; row < image.Rows; row++)
            for (int col = 0; col < image.Columns; col++)
            {
                byte pixelMask = mask.GetPixel(row, col, 0);
                // apply to all chnanels of image
                for (int channel = 0; channel < image.Channels; channel++)
                {
                    byte pixelImage = image.GetPixel(row, col, channel);
                    image.SetPixel(row, col, channel, (byte)(pixelImage & pixelMask));
                }
            }
    }
}
