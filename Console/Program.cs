using System;

using SharpVision;

public class Program
{
    public static void Main()
    {
        Matrix<byte> image = Sharp.ReadImage("Images/image4.png", ReadMode.Color);
        Matrix<byte> output = image.CopyStructureEmpty(3);

        Sharp.PrintHistogram(image);
        Sharp.ConvertColor(image, output, ColorConversion.RGBToHSI);
        Sharp.PrintHistogram(output);

        Sharp.SaveImage(output, Path.Combine(AppContext.BaseDirectory, "image4_hsi.png"));

        Console.WriteLine("Processing complete.");
    }
}
