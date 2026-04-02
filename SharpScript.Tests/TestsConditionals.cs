using SharpScript;

namespace SharpScript.Tests;

public class TestsConditionals
{
    [Fact]
    public void Test_If()
    {
        string code = @"
a = 0;
if (a == 0)
{
    a = 1;
}
";
        var variables = Utils.RunEnv(code);
        Assert.Equal(1, variables["a"]);
    }

    [Fact]
    public void Test_IfElse()
    {
        string code = @"
a = 1;
if (a == 0)
{
    result = false;
}
else
{
    result = true;
}
";
        var variables = Utils.RunEnv(code);
        Assert.Equal(true, variables["result"]);
    }

    [Fact]
    public void Test_IfElseIf()
    {
        string code = @"
a = 2;
if (a == 0)
{
    result = false;
}
else if (a == 1)
{
    result = false;
}
else if (a == 2)
{
    result = true;
}
else if (a == 3)
{
    result = false;
}
else if (a == 4)
{
    result = false;
}
";
        var variables = Utils.RunEnv(code);
        Assert.Equal(true, variables["result"]);
    }

    [Fact]
    public void Test_IfElseIIfElse()
    {
        string code = @"
a = 2;
if (a == 0)
{
    result = false;
}
else if (a == 1)
{
    result = false;
}
else
{
    result = true;
}
";
        var variables = Utils.RunEnv(code);
        Assert.Equal(true, variables["result"]);
    }
}
