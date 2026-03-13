using System;
using SharpVision.Core;

namespace SharpVision.Processing;

public static partial class Bitwise
{
    public static void BitwiseXor(Matrix<byte> source1, Matrix<byte> source2, Matrix<byte> destination)
    {
        if (source1.Data.Length != source2.Data.Length)
            throw new ArgumentException("Matrices must have the same dimensions and channels.");

        for (int i = 0; i < source1.Data.Length; i++)
            destination.Data[i] = (byte)(source1.Data[i] ^ source2.Data[i]);
    }
}
