using System;

namespace SharpVision.Core;

public static class ConsoleHistogram
{
    public static void Print(Matrix<byte> image)
    {
        // raw frequencies (256)
        int[] rawHistogram = new int[256];

        for (int i = 0; i < image.Data.Length; i++)
            rawHistogram[image.Data[i]]++;

        // 32 intervals
        int intervalCount = 32;
        int valuesPerInterval = 256 / intervalCount;
        int[] intervals = new int[intervalCount];

        int maxIntervalFrequency = 0;

        for (int i = 0; i < 256; i++)
        {
            int intervalIndex = i / valuesPerInterval;
            intervals[intervalIndex] += rawHistogram[i];

            if (intervals[intervalIndex] > maxIntervalFrequency)
                maxIntervalFrequency = intervals[intervalIndex];
        }

        // display in console
        int maxConsoleWidth = 50;
        Console.WriteLine("\n--------- Image Intensity Histogram ---------");

        if (maxIntervalFrequency == 0) return;

        for (int i = 0; i < intervalCount; i++)
        {
            int rangeStart = i * valuesPerInterval;
            int rangeEnd = rangeStart + valuesPerInterval - 1;

            double scaleFactor = (double)intervals[i] / maxIntervalFrequency;
            int barLength = (int)(scaleFactor * maxConsoleWidth);

            string label = $"{rangeStart:D3}-{rangeEnd:D3}";
            string bar = new string('█', barLength);

            Console.WriteLine($"[{label}] | {bar} {intervals[i]}");
        }
        Console.WriteLine("---------------------------------\n");
    }
}
