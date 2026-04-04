using System;

using SharpVision;

public class Program
{
    public static void Main()
    {
        Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "Output"));

        Matrix<byte> image = Sharp.ReadImage("Images/night.png", ReadMode.Color);
        Sharp.SaveImage(image, Path.Combine(AppContext.BaseDirectory, "Output/night.png"));

        Matrix<byte> imageHSI = new();
        Sharp.ConvertColor(image, imageHSI, ColorConversion.RGB2HSI);
        Sharp.SaveImage(imageHSI, Path.Combine(AppContext.BaseDirectory, "Output/night_hsi.png"));

        byte[] lower = [0, 200, 60];
        byte[] upper = [60, 255, 255];

        Matrix<byte> mask = new();
        Sharp.ThresholdRange(imageHSI, mask, lower, upper);
        Sharp.SaveImage(mask, Path.Combine(AppContext.BaseDirectory, "Output/mask.png"));

        Matrix<byte> output = new();
        Sharp.ApplyMask(image, output, mask);
        Sharp.SaveImage(output, Path.Combine(AppContext.BaseDirectory, "Output/night_mask.png"));

        Console.WriteLine("Processing complete.");
    }
}
