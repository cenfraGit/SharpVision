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

    [SharpFunction("Color", "Converts color spaces.")]
    private static object[] ConvertColor2(Matrix<byte> src, string conversionMode)
    {
        Matrix<byte> dst = new();
        ColorConversion mode;
        if (conversionMode == "RGB2GRAY")
            mode = ColorConversion.RGB2GRAY;
        else if (conversionMode == "GRAY2RGB")
            mode = ColorConversion.GRAY2RGB;
        else if (conversionMode == "RGB2HSI")
            mode = ColorConversion.RGB2HSI;
        else if (conversionMode == "HSI2RGB")
            mode = ColorConversion.HSI2RGB;
        else
            throw new ArgumentException($"Invalid conversionMode value: {conversionMode}");
        ConvertColor(src, dst, mode);
        return new object[] { dst };
    }
}
