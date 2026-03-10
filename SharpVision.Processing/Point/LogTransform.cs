using System;
using SharpVision.Core;

namespace SharpVision.Processing;

public static partial class Point
{
    public static void LogTransform(Matrix<byte> input, double c)
    {
        for (int i = 0; i < input.Data.Length; i++)
        {
            var r = input.Data[i];
            double s = (double)(c * Math.Log(1 + r));
            input.Data[i] = Utils.ClampToByte(s);
        }
    }
}
