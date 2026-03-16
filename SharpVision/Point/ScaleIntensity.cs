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
}
