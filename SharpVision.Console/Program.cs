using System;
using System.IO;
using SharpVision.Core;
using SharpVision.Processing;

namespace SharpVision.Console;

internal class SharpVisionClass
{
    public static async Task Main()
    {
        string imageName = "image2"; // png

        string inputPath = $"/home/cenfra/Pictures/{imageName}.png";
        string outputPath = $"/home/cenfra/Pictures/{imageName}_saved.png";

        Matrix<byte> myImage = await ImageIO.LoadGrayscaleAsync(inputPath);

        ConsoleHistogram.Print(myImage);

        // Point.Negative(myImage);
        // Point.Threshold(myImage, 60);
        // Point.ScaleIntensity(myImage, 2, 30);

        Point.Normalize(myImage, 0, 255, 0.05, 0.95);

        ConsoleHistogram.Print(myImage);

        await ImageIO.SaveAsync(myImage, outputPath);

        System.Console.WriteLine("Processing complete.");
    }
}
