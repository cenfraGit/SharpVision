namespace SharpVision;

public static partial class Sharp
{
    private static void GRAY2RGB(Matrix<byte> src, Matrix<byte> dst)
    {
        if (src.Channels != 1)
            throw new ArgumentException("Source matrix must be 1-channel.");

        dst.ReallocateIfNeeded(src.Rows, src.Columns, 3);

        int dstIndex = 0;
        for (int i = 0; i < src.Length; i++)
        {
            byte B = src.Data[i];

            dst.Data[dstIndex++] = B;
            dst.Data[dstIndex++] = B;
            dst.Data[dstIndex++] = B;
        }
    }
}
