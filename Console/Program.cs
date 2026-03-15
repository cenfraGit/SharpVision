using System;

using SharpVision;

public class Program
{
    public static void Main()
    {
        Matrix<byte> image = Sharp.ReadImage("Images/image4.png", ReadMode.Color);

        Sharp.PrintHistogram(image);
        Sharp.EqualizeHistogram(image, image);
        Sharp.PrintHistogram(image);

        var channelR = image.CopyStructureEmpty(channels: 1);
        var channelG = image.CopyStructureEmpty(channels: 1);
        var channelB = image.CopyStructureEmpty(channels: 1);

        Sharp.ExtractChannel<byte>(image, channelR, 0);
        Sharp.ExtractChannel<byte>(image, channelG, 1);
        Sharp.ExtractChannel<byte>(image, channelB, 2);

        // Sharp.PrintHistogram(channelR);
        // Sharp.PrintHistogram(channelG);
        // Sharp.PrintHistogram(channelB);

        // Sharp.SaveImage(channelR, Path.Combine(AppContext.BaseDirectory, "image4_R.png"));
        // Sharp.SaveImage(channelG, Path.Combine(AppContext.BaseDirectory, "image4_G.png"));
        // Sharp.SaveImage(channelB, Path.Combine(AppContext.BaseDirectory, "image4_B.png"));

        // Sharp.BitwiseNot(channelG, channelG);

        var output = image.CopyStructureEmpty(channels: 3);
        Sharp.InsertChannel(channelR, output, 0);
        Sharp.InsertChannel(channelG, output, 1);
        Sharp.InsertChannel(channelB, output, 2);

        Sharp.SaveImage(output, Path.Combine(AppContext.BaseDirectory, "image4_rebuilt.png"));

        Console.WriteLine("Processing complete.");
    }
}
