namespace SharpVision;

public static partial class Sharp
{
    public static void ExtractChannel<T>(Matrix<T> src, Matrix<T> dst, int channel) where T : unmanaged
    {
        if (dst.Channels != 1)
            throw new ArgumentException("Destination matrix must be 1-channel.");
        if (src.Rows != dst.Rows || src.Columns != dst.Columns)
            throw new ArgumentException("Matrix dimensions must match.");
        if (channel < 0 || channel >= src.Channels)
            throw new ArgumentOutOfRangeException(nameof(channel), "Invalid channel.");

        dst.ReallocateIfNeeded(src.Rows, src.Columns, 1);

        int totalPixels = dst.Length;
        int step = src.Channels;

        for (int i = 0; i < totalPixels; i++)
            dst.Data[i] = src.Data[(i * step) + channel];
    }

    public static void InsertChannel<T>(Matrix<T> src, Matrix<T> dst, int channel) where T : unmanaged
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

    [SharpFunction("Channel", "Extracts a channel from an image.")]
    private static object[] ExtractChannel2(Matrix<byte> src, int channel)
    {
        Matrix<byte> dst = new(src.Rows, src.Columns, 1);
        ExtractChannel(src, dst, channel);
        return new object[] { dst };
    }

    [SharpFunction("Channel", "Inserts a channel from an image.")]
    private static object[] InsertChannel2(Matrix<byte> src, Matrix<byte> dst, int channel)
    {
        InsertChannel(src, dst, channel);
        return new object[] { };
    }

    [SharpFunction("Matrix", "Creates a matrix.")]
    private static object[] CreateMatrix(int rows, int columns, int channels)
    {
        Matrix<byte> dst = new(rows, columns, channels);
        return new object[] { dst };
    }

    // [SharpFunction("Matrix", "Creates a matrix.")]
    // private static object[] CreateMatrix()
    // {
    //     Matrix<byte> dst = new();
    //     return new object[] { dst };
    // }
}
