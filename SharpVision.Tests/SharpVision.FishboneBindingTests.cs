using System.Reflection;
using Fishbone.Engine;
using Xunit;

namespace SharpVision.Tests;

public class SharpVisionFishboneBindingTests
{
    [Fact]
    public void CreateMatrix_ReturnsReusableMatrix()
    {
        var env = Run("""
let image = CreateMatrix();
""");

        Assert.IsType<Matrix<byte>>(env.GetValue("image"));
    }

    [Fact]
    public void ReadImage_MutatesExistingMatrixAndReturnsNoValues()
    {
        var env = Run($"""
let image = CreateMatrix();
let result = ReadImage(image, "{ScriptPath(GetNightImagePath())}", "color");
""");

        var image = Assert.IsType<Matrix<byte>>(env.GetValue("image"));
        Assert.Equal(3, image.Channels);
        Assert.Null(env.GetValue("result"));
    }

    [Fact]
    public void ConvertColor_ParsesEnumCaseInsensitivelyAndMutatesDestination()
    {
        var env = Run($"""
let image = CreateMatrix();
let gray = CreateMatrix();
ReadImage(image, "{ScriptPath(GetNightImagePath())}", "color");
ConvertColor(image, gray, "rgb2gray");
""");

        var gray = Assert.IsType<Matrix<byte>>(env.GetValue("gray"));
        Assert.Equal(1, gray.Channels);
    }

    [Fact]
    public void Threshold_ConvertsIntegerToByteParameterAndMutatesDestination()
    {
        var env = Run($"""
let image = CreateMatrix();
let mask = CreateMatrix();
ReadImage(image, "{ScriptPath(GetNightImagePath())}", "color");
Threshold(image, mask, 128);
""");

        var mask = Assert.IsType<Matrix<byte>>(env.GetValue("mask"));
        Assert.Equal(3, mask.Channels);
    }

    [Fact]
    public void ExtractChannel_WritesOneChannelDestination()
    {
        var env = Run($"""
let image = CreateMatrix();
let channel = CreateMatrix();
ReadImage(image, "{ScriptPath(GetNightImagePath())}", "color");
ExtractChannel(image, channel, 0);
""");

        var channel = Assert.IsType<Matrix<byte>>(env.GetValue("channel"));
        Assert.Equal(1, channel.Channels);
    }

    [Fact]
    public void InsertChannel_MutatesDestinationMatrix()
    {
        var env = Run($"""
let image = CreateMatrix();
let channel = CreateMatrix();
ReadImage(image, "{ScriptPath(GetNightImagePath())}", "color");
ExtractChannel(image, channel, 0);
InsertChannel(channel, image, 0);
""");

        var image = Assert.IsType<Matrix<byte>>(env.GetValue("image"));
        Assert.Equal(3, image.Channels);
    }

    [Fact]
    public void SaveImage_ReturnsNoValuesAndWritesFile()
    {
        string outputPath = Path.Combine(Path.GetTempPath(), $"sharpvision-save-{Guid.NewGuid():N}.png");

        try
        {
            var env = Run($"""
let image = CreateMatrix();
let result = ReadImage(image, "{ScriptPath(GetNightImagePath())}", "color");
SaveImage(image, "{ScriptPath(outputPath)}");
""");

            Assert.True(File.Exists(outputPath));
            Assert.Null(env.GetValue("result"));
        }
        finally
        {
            if (File.Exists(outputPath))
                File.Delete(outputPath);
        }
    }

    [Fact]
    public void SharpFunctions_ExceptCreateMatrix_ReturnVoid()
    {
        var sharpFunctionMethods = GetSharpFunctionMethods()
            .Where(method => method.Name != nameof(Sharp.CreateMatrix));

        Assert.All(sharpFunctionMethods, method => Assert.Equal(typeof(void), method.ReturnType));
    }

    [Fact]
    public void SharpFunctionDestinations_AreMarkedAsOutputs()
    {
        var methodsWithOutput = GetSharpFunctionMethods()
            .Where(method => method.Name != nameof(Sharp.CreateMatrix) && method.Name != nameof(Sharp.SaveImage));

        Assert.All(methodsWithOutput, method =>
            Assert.Contains(method.GetParameters(),
                            parameter => parameter.GetCustomAttribute<SharpOutputAttribute>() is not null));
    }

    [Fact]
    public void SharpFunctions_DoNotExposePrivateWrappers()
    {
        Assert.DoesNotContain(typeof(Sharp).GetMethods(BindingFlags.NonPublic | BindingFlags.Static),
                              method => method.Name.EndsWith("2", StringComparison.Ordinal)
                                  && method.GetCustomAttribute<SharpFunctionAttribute>() is not null);
    }

    private static Fishbone.Core.FishboneEnvironment Run(string script)
    {
        return FishboneEngine.Run(script, SharpFishbone.CreateConfiguration());
    }

    private static IEnumerable<MethodInfo> GetSharpFunctionMethods()
    {
        return typeof(Sharp)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(method => method.GetCustomAttribute<SharpFunctionAttribute>() is not null);
    }

    private static string GetNightImagePath()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory is not null)
        {
            string candidate = Path.Combine(directory.FullName, "Console", "Images", "night.png");
            if (File.Exists(candidate))
                return candidate;

            directory = directory.Parent;
        }

        throw new FileNotFoundException("Could not find Console/Images/night.png.");
    }

    private static string ScriptPath(string path)
    {
        return path.Replace('\\', '/');
    }
}
