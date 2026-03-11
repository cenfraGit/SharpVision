using System;
using SharpVision.Core;

namespace SharpVision.Processing;

public static partial class Point
{
    public static void Negative(Matrix<byte> input)
    {
        byte[] lut = new byte[256];
        for (int i = 0; i < lut.Length; i++)
        {
            int s = 256 - 1 - i; // L = 256
            lut[i] = Utils.ClampToByte(s);
        }

        Utils.ApplyLUT(input, lut);
    }
}
