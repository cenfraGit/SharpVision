namespace SharpVision;

// --------------------------------------------------------------------------------
// color conversion enum
// --------------------------------------------------------------------------------

public enum ColorConversion
{
    RGB2GRAY,
    GRAY2RGB,
    RGB2HSI,
    HSI2RGB
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
            case ColorConversion.RGB2GRAY:
                RGB2GRAY(src, dst);
                break;
            case ColorConversion.GRAY2RGB:
                GRAY2RGB(src, dst);
                break;
            case ColorConversion.RGB2HSI:
                RGB2HSI(src, dst);
                break;
            case ColorConversion.HSI2RGB:
                HSI2RGB(src, dst);
                break;
        }
    }
}
