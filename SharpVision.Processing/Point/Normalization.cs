using System;
using SharpVision.Core;

namespace SharpVision.Processing;

public static partial class Point
{
    public static void Normalize(this Matrix<byte> image,
                                 byte lowerNew = 0,
                                 byte upperNew = 255,
                                 double lowerPercentile = 0.0,
                                 double upperPercentile = 1.0)
    {
        int totalPixels = image.Data.Length;
        int[] histogram = new int[256];

        for (int i = 0; i < totalPixels; i++)
            histogram[image.Data[i]]++;

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

        // apply transformation
        for (int i = 0; i < totalPixels; i++)
        {
            double r = image.Data[i];
            double s = (r - lowerOld) * scale + lowerNew;
            image.Data[i] = Utils.ClampToByte(s);
        }
    }
}
