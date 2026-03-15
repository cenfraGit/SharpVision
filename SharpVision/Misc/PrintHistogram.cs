namespace SharpVision;

public static partial class Sharp
{
    public static void PrintHistogram(Matrix<byte> src)
    {
        int channels = src.Channels;
        int intervalCount = 32;
        int valuesPerInterval = 256 / intervalCount;

        int[][] intervals = new int[channels][];
        for (int c = 0; c < channels; c++)
            intervals[c] = new int[intervalCount];

        int maxFrequency = 0;

        for (int i = 0; i < src.Length; i += channels)
        {
            for (int c = 0; c < channels; c++)
            {
                byte intensity = src.Data[i + c];
                int binIndex = intensity / valuesPerInterval;

                intervals[c][binIndex]++;

                if (intervals[c][binIndex] > maxFrequency)
                    maxFrequency = intervals[c][binIndex];
            }
        }

        if (maxFrequency == 0) return;

        int printHeight = 15;
        Console.WriteLine($"\n--------- Image Intensity Histogram ({channels} Channel(s)) ---------");

        for (int row = printHeight; row > 0; row--)
        {
            for (int c = 0; c < channels; c++)
            {
                for (int bin = 0; bin < intervalCount; bin++)
                {
                    double scaleFactor = (double)intervals[c][bin] / maxFrequency;
                    int barHeight = (int)(scaleFactor * printHeight);

                    if (barHeight >= row)
                        Console.Write("█");
                    else
                        Console.Write(" ");
                }

                // channel separator
                if (c < channels - 1)
                    Console.Write("  |  ");
            }
            Console.WriteLine();
        }

        for (int c = 0; c < channels; c++)
        {
            string xAxis = "0".PadRight(intervalCount - 3) + "255";
            Console.Write(xAxis);

            if (c < channels - 1)
                Console.Write("  |  ");
        }
        Console.WriteLine("\n----------------------------------------------------------\n");
    }
}
