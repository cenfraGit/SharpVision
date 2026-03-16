namespace SharpVision;

public static partial class Sharp
{
    public static void EqualizeHistogram(Matrix<byte> src, Matrix<byte> dst)
    {
        dst.ReallocateIfNeeded(src.Rows, src.Columns, src.Channels);

        int totalPixels = src.Length;
        int[] histogram = new int[256];

        for (int i = 0; i < totalPixels; i++)
            histogram[src.Data[i]]++;

        double scaleFactor = 255.0 / totalPixels;

        // calculate PDF using running total and use to build CDF(k) (s)
        byte[] lut = new byte[256];
        int runningSum = 0;
        for (int i = 0; i < lut.Length; i++)
        {
            runningSum += histogram[i];
            double s = runningSum * scaleFactor;
            lut[i] = Utils.ClampToByte(s);
        }

        Utils.ApplyLUT(src, dst, lut);
    }
}
