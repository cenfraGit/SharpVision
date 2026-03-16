namespace SharpVision;

public static partial class Sharp
{
    public static void ThresholdRange(Matrix<byte> src,
                                      Matrix<byte> dst,
                                      ReadOnlySpan<byte> lower,
                                      ReadOnlySpan<byte> upper)
    {
        if (lower.Length != src.Channels || upper.Length != src.Channels)
        {
            throw new ArgumentException("Lower and upper bounds must match source channels.");
        }

        dst.ReallocateIfNeeded(src.Rows, src.Columns, 1);

        int dstIndex = 0;
        int channels = src.Channels;

        for (int i = 0; i < src.Length; i += channels)
        {
            bool isMatch = true;

            for (int c = 0; c < channels; c++)
            {
                byte pixelValue = src.Data[i + c];

                // if single channel is out of bounds, whole pixel is discarded
                if (pixelValue < lower[c] || pixelValue > upper[c])
                {
                    isMatch = false;
                    break;
                }
            }

            dst.Data[dstIndex++] = isMatch ? (byte)255 : (byte)0;
        }
    }
}
