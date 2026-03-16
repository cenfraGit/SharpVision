namespace SharpVision;

public static partial class Sharp
{
    public static void Threshold(Matrix<byte> src, Matrix<byte> dst, byte threshold)
    {
        dst.ReallocateIfNeeded(src.Rows, src.Columns, src.Channels);

        byte[] lut = new byte[256];
        for (int i = 0; i < lut.Length; i++)
        {
            lut[i] = (i >= threshold) ? (byte)255 : (byte)0;
        }
        Utils.ApplyLUT(src, dst, lut);
    }
}
