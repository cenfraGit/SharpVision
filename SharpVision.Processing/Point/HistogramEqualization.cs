using System;
using SharpVision.Core;

namespace SharpVision.Processing;

public static partial class Point
{
    public static void HistogramEqualization(Matrix<byte> input)
    {
        int totalPixels = input.Data.Length;
        int[] histogram = new int[256];

        for (int i = 0; i < totalPixels; i++)
            histogram[input.Data[i]]++;

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

        Utils.ApplyLUT(input, lut);
    }
}
