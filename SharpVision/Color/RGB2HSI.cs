namespace SharpVision;

public static partial class Sharp
{
    private static void RGB2HSI(Matrix<byte> src, Matrix<byte> dst)
    {
        dst.ReallocateIfNeeded(src.Rows, src.Columns, 3);

        for (int i = 0; i < src.Length; i += 3)
        {
            // fetch byte values
            byte Br = src.Data[i];
            byte Bg = src.Data[i + 1];
            byte Bb = src.Data[i + 2];

            // convert to double for use in calculations
            // (and maybe avoid overflows?)
            double r = Br;
            double g = Bg;
            double b = Bb;

            // -------------------- hue -------------------- //

            double nominator = ( (r - g) + (r - b) ) / 2;
            double denominator = Math.Pow(r - g, 2) + (r - b) * (g - b);
            denominator = Math.Sqrt(denominator);

            if (denominator == 0) denominator = 0.001;

            double ratio = Math.Clamp(nominator / denominator, -1.0, 1.0);
            double angle = Math.Acos(ratio) * (180.0 / Math.PI);

            double hue;
            if (b <= g)
                hue = angle;
            else
                hue = 360 - angle;

            // ----------------- saturation ----------------- //

            double sum = r + g + b;

            double saturation = 0;

            if (sum > 0)
            {
                double min = Math.Min(r, Math.Min(g, b));
                saturation = 1.0 - (3.0 / sum) * min;
            }

            saturation = saturation * 255.0;

            // ----------------- intensity ----------------- //

            double intensity = (r + g + b) / 3;

            // ------------------- write ------------------- //

            dst.Data[i] = (byte)Math.Round(hue / 2.0);
            dst.Data[i + 1] = Utils.ClampToByte(saturation);
            dst.Data[i + 2] = Utils.ClampToByte(intensity);
        }
    }
}
