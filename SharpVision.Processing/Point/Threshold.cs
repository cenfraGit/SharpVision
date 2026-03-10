using System;
using SharpVision.Core;

namespace SharpVision.Processing;

public static partial class Point
{
    public static void Threshold(Matrix<byte> input, byte threshold)
    {
        for (int i = 0; i < input.Data.Length; i++)
        {
            var r = input.Data[i];
            input.Data[i] = (r >= threshold) ? (byte)255 : (byte)0;
        }
    }
}
