using System;

using SharpVision;

public class Program
{
    public static void Main()
    {
        Matrix<byte> image = Sharp.ReadImage("Images/image4.png", ReadMode.Color);
        Matrix<byte> output = image.CopyStructureEmpty(3);

        Sharp.PrintHistogram(image);
        Sharp.ConvertColor(image, output, ColorConversion.RGB2HSI);
        Sharp.SaveImage(output, Path.Combine(AppContext.BaseDirectory, "image4_hsi.png"));
        Sharp.PrintHistogram(output);
        Sharp.ConvertColor(output, output, ColorConversion.HSI2RGB);
        Sharp.SaveImage(output, Path.Combine(AppContext.BaseDirectory, "image4_rgb.png"));
        Sharp.PrintHistogram(output);

        Sharp.SaveImage(image, Path.Combine(AppContext.BaseDirectory, "image4.png"));

        Console.WriteLine("Processing complete.");
    }
}
