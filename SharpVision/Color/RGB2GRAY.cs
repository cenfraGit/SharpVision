namespace SharpVision;

public static partial class Sharp
{
    private static void RGB2GRAY(Matrix<byte> src, Matrix<byte> dst)
    {
        dst.ReallocateIfNeeded(src.Rows, src.Columns, 1);

        int dstIndex = 0;
        for (int i = 0; i < src.Length; i += 3)
        {
            byte Br = src.Data[i];
            byte Bg = src.Data[i + 1];
            byte Bb = src.Data[i + 2];

            double R = Br;
            double G = Bg;
            double B = Bb;

            double gray = 0.299*R + 0.587*G + 0.114*B;

            dst.Data[dstIndex++] = Utils.ClampToByte(gray);
        }
    }
}
