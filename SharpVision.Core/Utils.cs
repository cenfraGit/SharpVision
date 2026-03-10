using System;

namespace SharpVision.Core;

public static class Utils
{
    public static byte ClampToByte(double value)
    {
        if (value < 0) return 0;
        if (value > 255) return 255;
        return (byte)value;
    }
}
