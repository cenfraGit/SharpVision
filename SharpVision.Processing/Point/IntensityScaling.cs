using System;
using SharpVision.Core;

namespace SharpVision.Processing;

public static partial class Point
{
    public static void ScaleIntensity(Matrix<byte> input, double alpha, double beta)
    {
        for (int i = 0; i < input.Data.Length; i++)
        {
            var r = input.Data[i];
            double s = alpha * r + beta;
            input.Data[i] = Utils.ClampToByte(s);
        }
    }
}
