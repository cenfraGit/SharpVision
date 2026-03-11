using System;
using SharpVision.Core;

namespace SharpVision.Processing;

public static partial class Point
{
    public static void Threshold(Matrix<byte> input, byte threshold)
    {
        byte[] lut = new byte[256];
        for (int i = 0; i < lut.Length; i++)
        {
            lut[i] = (i >= threshold) ? (byte)255 : (byte)0;
        }
        Utils.ApplyLUT(input, lut);
    }
}
