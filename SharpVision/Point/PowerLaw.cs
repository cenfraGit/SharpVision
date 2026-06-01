namespace SharpVision;

public static partial class Sharp
{
    [SharpFunction("Point", "Applies a power law transformation.")]
    public static void PowerLaw(Matrix<byte> src, [SharpOutput] Matrix<byte> dst, double c, double gamma)
    {
        dst.ReallocateIfNeeded(src.Rows, src.Columns, src.Channels);

        byte[] lut = new byte[256];
        for (int i = 0; i < lut.Length; i++)
        {
            double s = (double)(c * Math.Pow(i, gamma));
            lut[i] = Utils.ClampToByte(s);
        }

        Utils.ApplyLUT(src, dst, lut);
    }
}
