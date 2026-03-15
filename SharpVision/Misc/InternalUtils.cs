using System;
using System.Runtime.CompilerServices;

namespace SharpVision;

internal static class Utils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ClampToByte(double value)
    {
        if (value < 0) return 0;
        if (value > 255) return 255;
        return (byte)value;
    }

    public static void ApplyLUT(Matrix<byte> src, Matrix<byte> dst, byte[] lut)
    {
        if (lut == null || lut.Length != 256)
            throw new ArgumentException("LUT must contain exactly 256 elements.", nameof(lut));

        if (src.Length != dst.Length)
            throw new ArgumentException("Matrices must have the same dimensions and channels.");

        for (int i = 0; i < dst.Length; i++)
            dst.Data[i] = lut[src.Data[i]];
    }
}
