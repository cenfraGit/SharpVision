namespace SharpVision;

public static partial class Sharp
{
    public static void Log(Matrix<byte> src, Matrix<byte> dst, double c)
    {
        dst.ReallocateIfNeeded(src.Rows, src.Columns, src.Channels);

        byte[] lut = new byte[256];
        for (int i = 0; i < lut.Length; i++)
        {
            double s = (double)(c * Math.Log(1 + i));
            lut[i] = Utils.ClampToByte(s);
        }

        Utils.ApplyLUT(src, dst, lut);
    }
}
