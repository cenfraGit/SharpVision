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
}
