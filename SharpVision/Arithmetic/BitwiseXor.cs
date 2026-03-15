namespace SharpVision;

public static partial class Sharp
{
    public static void BitwiseXor(Matrix<byte> src1, Matrix<byte> src2, Matrix<byte> dst)
    {
        if (src1.Length != src2.Length)
            throw new ArgumentException("Matrices must have the same dimensions and channels.");

        for (int i = 0; i < src1.Length; i++)
            dst.Data[i] = (byte)(src1.Data[i] ^ src2.Data[i]);
    }
}
