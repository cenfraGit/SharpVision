using System;
using System.Runtime.CompilerServices;

namespace SharpVision.Core;

public static class Utils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ClampToByte(double value)
    {
        if (value < 0) return 0;
        if (value > 255) return 255;
        return (byte)value;
    }

    public static void ApplyLUT(Matrix<byte> input, byte[] lut)
    {
        if (lut == null || lut.Length != 256)
                throw new ArgumentException("LUT must contain exactly 256 elements.",
                                            nameof(lut));

        for (int i = 0; i < input.Data.Length; i++)
            input.Data[i] = lut[input.Data[i]];
    }

    public static Matrix<byte> ConvertToGrayscale(Matrix<byte> input)
    {
        if (input.Channels != 3)
            throw new ArgumentException("Input matrix must have 3 channels.");

        var output = new Matrix<byte>(input.Rows, input.Columns, 1);

        for (int row = 0; row < input.Rows - 1; row++)
            for (int col = 0; col < input.Columns; col++)
            {
                var r = input.GetPixel(row, col, 0);
                var g = input.GetPixel(row, col, 1);
                var b = input.GetPixel(row, col, 2);
                double newValue = 0.299 * r + 0.587 * g + 0.114 * b;
                output.SetPixel(row, col, 0, Utils.ClampToByte(newValue));
            }

        return output;
    }
}
