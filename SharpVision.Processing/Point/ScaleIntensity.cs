using System;
using SharpVision.Core;

namespace SharpVision.Processing;

public static partial class Point
{
    public static void ScaleIntensity(Matrix<byte> input, double alpha, double beta)
    {
        byte[] lut = new byte[256];
        for (int i = 0; i < lut.Length; i++)
        {
            double s = alpha * i + beta;
            lut[i] = Utils.ClampToByte(s);
        }

        Utils.ApplyLUT(input, lut);
    }
}
