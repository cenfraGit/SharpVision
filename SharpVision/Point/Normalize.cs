namespace SharpVision;

public static partial class Sharp
{
    public static void Normalize(Matrix<byte> src,
                                 Matrix<byte> dst,
                                 byte lowerNew = 0,
                                 byte upperNew = 255,
                                 double lowerPercentile = 0.0,
                                 double upperPercentile = 1.0)
    {
        dst.ReallocateIfNeeded(src.Rows, src.Columns, src.Channels);

        int totalPixels = src.Length;
        int[] histogram = new int[256];

        for (int i = 0; i < totalPixels; i++)
            histogram[src.Data[i]]++;

        // pixel count thresholds for percentiles
        int lowerThresholdCount = (int)(totalPixels * lowerPercentile);
        int upperThresholdCount = (int)(totalPixels * upperPercentile);

        byte lowerOld = 0;
        byte upperOld = 255;
        int runningSum = 0;

        // calculate distribution to find percentile bounds
        for (int i = 0; i < 256; i++)
        {
            runningSum += histogram[i];

            if (lowerOld == 0 && runningSum >= lowerThresholdCount)
                lowerOld = (byte)i;

            if (upperOld == 255 && runningSum >= upperThresholdCount)
            {
                upperOld = (byte)i;
                break;
            }
        }

        // if percentiles are the same
        if (lowerOld >= upperOld) return;

        double scale = (double)(upperNew - lowerNew) / (upperOld - lowerOld);

        // calculate transformation
        byte[] lut = new byte[256];
        for (int i = 0; i < lut.Length; i++)
        {
            double s = (i - lowerOld) * scale + lowerNew;
            lut[i] = Utils.ClampToByte(s);
        }

        Utils.ApplyLUT(src, dst, lut);
    }

    [SharpFunction("Point", "Scales values to lower and upper boundaries.")]
    private static object[] Normalize2(Matrix<byte> src,
                                       byte lowerNew,
                                       byte upperNew,
                                       double lowerPercentile,
                                       double upperPercentile)
    {
        Matrix<byte> dst = new();
        Normalize(src, dst, lowerNew, upperNew, lowerPercentile, upperPercentile);
        return new object[] { dst };
    }
}
