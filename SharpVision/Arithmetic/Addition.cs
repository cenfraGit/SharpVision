namespace SharpVision;

public static partial class Sharp
{
    public static void Addition(Matrix<byte> src1, Matrix<byte> src2, Matrix<byte> dst)
    {
        if (src1.Length != src2.Length)
            throw new ArgumentException("Matrices must have the same dimensions and channels.");

        dst.ReallocateIfNeeded(src1.Rows, src1.Columns, src1.Channels);

        for (int row = 0; row < dst.Rows; row++)
            for (int col = 0; col < dst.Columns; col++)
            {
                for (int channel = 0; channel < dst.Channels; channel++)
                {
                    var valueA = src1.GetPixel(row, col, channel);
                    var valueB = src2.GetPixel(row, col, channel);
                    dst.SetPixel(row, col, channel, Utils.ClampToByte(valueA + valueB));
                }
            }
    }

    [SharpFunction("Arithmetic", "Adds two matrices.")]
    private static object[] Addition2(Matrix<byte> src1, Matrix<byte> src2)
    {
        Matrix<byte> dst = new();
        Addition(src1, src2, dst);
        return new object[] { dst };
    }
}
