using System;
using SharpVision.Core;

namespace SharpVision.Processing;

public static partial class Point
{
    public static void Negative(Matrix<byte> input)
    {
        for (int i = 0; i < input.Data.Length; i++)
        {
            var r = input.Data[i];
            int s = 255 - 1 - r; // L = 255
            input.Data[i] = Utils.ClampToByte(s);
        }
    }
}
