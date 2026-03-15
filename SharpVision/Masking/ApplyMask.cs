namespace SharpVision;

public static partial class Sharp
{
    public static void ApplyMask(Matrix<byte> src, Matrix<byte> dst, Matrix<byte> mask)
    {
        if (src.Rows != mask.Rows || src.Columns != mask.Columns)
            throw new ArgumentException("Matrix and mask must have same dimensions.");

        if (mask.Channels != 1)
            throw new ArgumentException("Mask must be a 1-channel matrix.");

        for (int row = 0; row < src.Rows; row++)
            for (int col = 0; col < src.Columns; col++)
            {
                byte pixelMask = mask.GetPixel(row, col, 0);
                // apply to all chnanels of src
                for (int channel = 0; channel < src.Channels; channel++)
                {
                    byte pixelImage = src.GetPixel(row, col, channel);
                    dst.SetPixel(row, col, channel, (byte)(pixelImage & pixelMask));
                }
            }
    }
}
