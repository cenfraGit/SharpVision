using System;
using SharpVision.Core;

namespace SharpVision.Processing;

public static partial class Point
{
    public static void LogTransform(Matrix<byte> input, double c)
    {
        byte[] lut = new byte[256];
        for (int i = 0; i < lut.Length; i++)
        {
            double s = (double)(c * Math.Log(1 + i));
            lut[i] = Utils.ClampToByte(s);
        }

        Utils.ApplyLUT(input, lut);
    }
}
