namespace SharpVision;

public static partial class Sharp
{
    [SharpFunction("Channel", "Extracts a channel from an image.")]
    public static void ExtractChannel(Matrix<byte> src, [SharpOutput] Matrix<byte> dst, int channel)
    {
        if (channel < 0 || channel >= src.Channels)
            throw new ArgumentOutOfRangeException(nameof(channel), "Invalid channel.");

        dst.ReallocateIfNeeded(src.Rows, src.Columns, 1);

        int totalPixels = dst.Length;
        int step = src.Channels;

        for (int i = 0; i < totalPixels; i++)
            dst.Data[i] = src.Data[(i * step) + channel];
    }

    [SharpFunction("Channel", "Inserts a channel from an image.")]
    public static void InsertChannel(Matrix<byte> src, [SharpOutput] Matrix<byte> dst, int channel)
    {
        if (src.Channels != 1)
            throw new ArgumentException("Source matrix must be 1-channel.");
        if (src.Rows != dst.Rows || src.Columns != dst.Columns)
            throw new ArgumentException("Matrix dimensions must match.");
        if (channel < 0 || channel >= dst.Channels)
            throw new ArgumentOutOfRangeException(nameof(channel), "Invalid channel.");

        int totalPixels = src.Length;
        int step = dst.Channels;

        for (int i = 0; i < totalPixels; i++)
            dst.Data[(i * step) + channel] = src.Data[i];
    }

    [SharpFunction("Matrix", "Creates a matrix.")]
    public static Matrix<byte> CreateMatrix()
    {
        return new Matrix<byte>();
    }
}
