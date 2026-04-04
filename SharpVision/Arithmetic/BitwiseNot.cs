namespace SharpVision;

public static partial class Sharp
{
    public static void BitwiseNot(Matrix<byte> src, Matrix<byte> dst)
    {
        dst.ReallocateIfNeeded(src.Rows, src.Columns, src.Channels);

        byte[] lut = new byte[256];
        for (int i = 0; i < lut.Length; i++)
        {
            int s = 256 - 1 - i;
            lut[i] = Utils.ClampToByte(s);
        }

        Utils.ApplyLUT(src, dst, lut);
    }

    [SharpFunction("Arithmetic", "Performs bitwise not.")]
    private static object[] BitwiseNot2(Matrix<byte> src)
    {
        Matrix<byte> dst = new();
        BitwiseNot(src, dst);
        return new object[] { dst };
    }
}
