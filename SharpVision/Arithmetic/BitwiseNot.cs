namespace SharpVision;

public static partial class Sharp
{
    public static void BitwiseNot(Matrix<byte> src, Matrix<byte> dst)
    {
        byte[] lut = new byte[256];
        for (int i = 0; i < lut.Length; i++)
        {
            int s = 256 - 1 - i;
            lut[i] = Utils.ClampToByte(s);
        }

        Utils.ApplyLUT(src, dst, lut);
    }
}
