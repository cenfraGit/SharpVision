namespace SharpVision;

public static partial class Sharp
{
    public static void ScaleIntensity(Matrix<byte> src, Matrix<byte> dst, double alpha, double beta)
    {
        dst.ReallocateIfNeeded(src.Rows, src.Columns, src.Channels);

        byte[] lut = new byte[256];
        for (int i = 0; i < lut.Length; i++)
        {
            double s = alpha * i + beta;
            lut[i] = Utils.ClampToByte(s);
        }

        Utils.ApplyLUT(src, dst, lut);
    }

    [SharpFunction("Point", "Modifies brightness and contrast.")]
    private static object[] ScaleIntensity2(Matrix<byte> src, double alpha, double beta)
    {
        Matrix<byte> dst = new();
        ScaleIntensity(src, dst, alpha, beta);
        return new object[] { dst };
    }
}
