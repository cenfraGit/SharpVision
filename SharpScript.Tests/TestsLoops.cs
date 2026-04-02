using SharpScript;

namespace SharpScript.Tests;

public class TestsLoops
{
    [Fact]
    public void Test_While()
    {
        string code = @"
a = 0;
while (a <= 10)
{
    a = a + 1;
}
";
        var variables = Utils.RunEnv(code);
        Assert.Equal(11, variables["a"]);
    }
}
