namespace SharpVision;

public static partial class Sharp
{
    private static void HSI2RGB(Matrix<byte> src, Matrix<byte> dst)
    {
        dst.ReallocateIfNeeded(src.Rows, src.Columns, 3);

        for (int i = 0; i < src.Length; i += 3)
        {
            // fetch byte values
            byte Bh = src.Data[i];
            byte Bs = src.Data[i + 1];
            byte Bi = src.Data[i + 2];

            double H = Bh * 2; // we'd divided by 2 since 360 cant fit in byte
            double S = Bs / 255.0; // normalize
            double I = Bi / 255.0; // normalize

            double R = 0.0;
            double G = 0.0;
            double B = 0.0;

            // ----------------- RG sector ----------------- //

            if (0 <= H && H < 120)
            {
                double hRad = H * (Math.PI / 180.0);
                double offsetRad = (60.0 - H) * (Math.PI / 180.0);
                B = I * (1 - S);
                R = I * ( 1 + ( (S * Math.Cos(hRad)) / (Math.Cos(offsetRad)) ) );
                G = 3 * I - (R + B);
            }

            // ----------------- GB sector ----------------- //

            else if (120 <= H && H < 240)
            {
                H = H - 120;
                double hRad = H * (Math.PI / 180.0);
                double offsetRad = (60.0 - H) * (Math.PI / 180.0);
                R = I * (1 - S);
                G = I * (1 + ( (S * Math.Cos(hRad)) / (Math.Cos(offsetRad))) );
                B = 3 * I - (R + G);
            }

            // ----------------- BR sector ----------------- //

            else if (240 <= H || H <= 360)
            {
                H = H - 240;
                double hRad = H * (Math.PI / 180.0);
                double offsetRad = (60.0 - H) * (Math.PI / 180.0);
                G = I * (1 - S);
                B = I * (1 + ((S * Math.Cos(hRad)) / (Math.Cos(offsetRad)) ));
                R = 3 * I - (G + B);
            }

            // ------------------- write ------------------- //

            // scale back up
            R = R * 255.0;
            G = G * 255.0;
            B = B * 255.0;

            dst.Data[i] = Utils.ClampToByte(R);
            dst.Data[i + 1] = Utils.ClampToByte(G);
            dst.Data[i + 2] = Utils.ClampToByte(B);
        }
    }
}
