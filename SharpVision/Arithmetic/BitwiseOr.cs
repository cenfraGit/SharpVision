namespace SharpVision;

public static partial class Sharp
{
    public static void BitwiseOr(Matrix<byte> src1, Matrix<byte> src2, Matrix<byte> dst)
    {
        if (src1.Length != src2.Length)
            throw new ArgumentException("Matrices must have the same dimensions and channels.");

        dst.ReallocateIfNeeded(src1.Rows, src1.Columns, src1.Channels);

        for (int i = 0; i < src1.Length; i++)
            dst.Data[i] = (byte)(src1.Data[i] | src2.Data[i]);
    }

    [SharpFunction("Arithmetic", "Performs bitwise or.")]
    private static object[] BitwiseOr2(Matrix<byte> src1, Matrix<byte> src2)
    {
        Matrix<byte> dst = new();
        BitwiseOr(src1, src2, dst);
        return new object[] { dst };
    }
}
