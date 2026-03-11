using System;
using SharpVision.Core;

namespace SharpVision.Processing;

public static partial class Arithmetic
{
    public static void Subtract(Matrix<byte> inputA, Matrix<byte> inputB, out Matrix<byte> result)
    {
        if (inputA.Channels != inputB.Channels)
            throw new ArgumentException("Input channels don't match.");

        if (inputA.Rows != inputB.Rows)
            throw new ArgumentException("Input rows don't match.");

        if (inputA.Columns != inputB.Columns)
            throw new ArgumentException("Input columns don't match.");
        
        result = new(inputA.Rows, inputA.Columns, inputA.Channels);

        for (int row = 0; row < result.Rows; row++)
            for (int col = 0; col < result.Columns; col++)
            {
                for (int channel = 0; channel < result.Channels; channel++)
                {
                    var valueA = inputA.GetPixel(row, col, channel);
                    var valueB = inputB.GetPixel(row, col, channel);
                    result.SetPixel(row, col, channel, Utils.ClampToByte(valueA - valueB));
                }
            }
    }
}
