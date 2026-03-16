namespace SharpVision;

// --------------------------------------------------------------------------------
// color conversion enum
// --------------------------------------------------------------------------------

public enum ColorConversion
{
    RGBToHSI
}

// --------------------------------------------------------------------------------
// convert color
// --------------------------------------------------------------------------------

public static partial class Sharp
{
    public static void ConvertColor(Matrix<byte> src, Matrix<byte> dst, ColorConversion conversionMode)
    {
        switch (conversionMode)
        {
            case ColorConversion.RGBToHSI:
                RGBToHSI(src, dst);
                break;
        }
    }
}
