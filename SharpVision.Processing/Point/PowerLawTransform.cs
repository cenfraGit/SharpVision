using System;
using SharpVision.Core;

namespace SharpVision.Processing;

public static partial class Point
{
    public static void PowerLawTransform(Matrix<byte> input, double c, double gamma)
    {
        byte[] lut = new byte[256];
        for (int i = 0; i < lut.Length; i++)
        {
            double s = (double)(c * Math.Pow(i, gamma));
            lut[i] = Utils.ClampToByte(s);
        }

        Utils.ApplyLUT(input, lut);
    }
}
