using SharpScript;

namespace SharpScript.Tests;

public class TestsExpr
{
    // helper for getting environment variables
    internal Dictionary<string, object?> RunEnv(string code)
    {
        var env = new SharpScriptEnvironment(code);
        env.Run();
        return env.Variables;
    }

    [Fact]
    public void Test_StoreInteger()
    {
        string code = "x = 10;";
        var variables = RunEnv(code);
        Assert.True(variables.ContainsKey("x"));
        Assert.Equal(10, variables["x"]);
    }

    [Fact]
    public void Test_StoreFloat()
    {
        string code = "x = 8.14; y = 10.0;";
        var variables = RunEnv(code);
        Assert.True(variables.ContainsKey("x"));
        Assert.True(variables.ContainsKey("y"));
        Assert.Equal((float)8.14, variables["x"]);
        Assert.Equal((float)10, variables["y"]);
    }

    [Fact]
    public void Test_StoreString()
    {
        string code = "x = \"hello\";";
        var variables = RunEnv(code);
        Assert.True(variables.ContainsKey("x"));
        Assert.Equal("hello", variables["x"]);
    }

    [Fact]
    public void Test_StoreBool()
    {
        string code = "x = true; y = false;";
        var variables = RunEnv(code);
        Assert.True(variables.ContainsKey("x"));
        Assert.True(variables.ContainsKey("y"));
        Assert.Equal(true, variables["x"]);
        Assert.Equal(false, variables["y"]);
    }

    [Fact]
    public void Test_StoreId()
    {
        string code = "x = 1; y = x;";
        var variables = RunEnv(code);
        Assert.True(variables.ContainsKey("x"));
        Assert.True(variables.ContainsKey("y"));
        Assert.Equal(1, variables["y"]);
    }

    [Fact]
    public void Test_StoreIdByValue()
    {
        string code = "x = 1; y = x; x = 2;";
        var variables = RunEnv(code);
        Assert.True(variables.ContainsKey("x"));
        Assert.True(variables.ContainsKey("y"));
        Assert.Equal(1, variables["y"]);
    }

    [Fact]
    public void Test_StoreParenthesesExpr()
    {
        string code = "x = (1); y = (x + 2);";
        var variables = RunEnv(code);
        Assert.True(variables.ContainsKey("x"));
        Assert.True(variables.ContainsKey("y"));
        Assert.Equal(1, variables["x"]);
        Assert.Equal(3, variables["y"]);
    }

    [Fact]
    public void Test_VisitComparisonEQ()
    {
        string code = @"
a = 5 == 5;
b = 10 == 5;
c = true == true;
d = 10.1 == 10.1;
e = 10.1 == 10.0;";
        var variables = RunEnv(code);
        Assert.Equal(true, variables["a"]);
        Assert.Equal(false, variables["b"]);
        Assert.Equal(true, variables["c"]);
        Assert.Equal(true, variables["d"]);
        Assert.Equal(false, variables["e"]);
    }

    [Fact]
    public void Test_VisitComparisonNEQ()
    {
        string code = @"
a = 5 != 5;
b = 10 != 5;
c = true != true;
d = 10.1 != 10.1;
e = 10.1 != 10.0;";
        var variables = RunEnv(code);
        Assert.Equal(false, variables["a"]);
        Assert.Equal(true, variables["b"]);
        Assert.Equal(false, variables["c"]);
        Assert.Equal(false, variables["d"]);
        Assert.Equal(true, variables["e"]);
    }

    [Fact]
    public void Test_VisitComparisonGT()
    {
        string code = @"
a = 10 > 5;
b = 5 > 10;
c = 5 > 5";
        var variables = RunEnv(code);
        Assert.Equal(true, variables["a"]);
        Assert.Equal(false, variables["b"]);
        Assert.Equal(false, variables["c"]);
    }

    [Fact]
    public void Test_VisitComparisonGE()
    {
        string code = @"
a = 10 >= 5;
b = 5 >= 10;
c = 5 >= 5;";
        var variables = RunEnv(code);
        Assert.Equal(true, variables["a"]);
        Assert.Equal(false, variables["b"]);
        Assert.Equal(true, variables["c"]);
    }

    [Fact]
    public void Test_VisitComparisonLT()
    {
        string code = @"
a = 10 < 5;
b = 5 < 10;
c = 5 < 5;";
        var variables = RunEnv(code);
        Assert.Equal(false, variables["a"]);
        Assert.Equal(true, variables["b"]);
        Assert.Equal(false, variables["c"]);
    }

    [Fact]
    public void Test_VisitComparisonLE()
    {
        string code = @"
a = 10 <= 5;
b = 5 <= 10;
c = 5 <= 5;";
        var variables = RunEnv(code);
        Assert.Equal(false, variables["a"]);
        Assert.Equal(true, variables["b"]);
        Assert.Equal(true, variables["c"]);
    }

    [Fact]
    public void Test_VisitAnd()
    {
        string code = @"
a = true and true;
b = true and false;
c = false and false;";
        var variables = RunEnv(code);
        Assert.Equal(true, variables["a"]);
        Assert.Equal(false, variables["b"]);
        Assert.Equal(false, variables["c"]);
    }

    [Fact]
    public void Test_VisitOr()
    {
        string code = @"
a = true or true;
b = true or false;
c = false or false;";
        var variables = RunEnv(code);
        Assert.Equal(true, variables["a"]);
        Assert.Equal(true, variables["b"]);
        Assert.Equal(false, variables["c"]);
    }

    [Fact]
    public void Test_VisitXor()
    {
        string code = @"
a = true xor true;
b = true xor false;
c = false xor false;";
        var variables = RunEnv(code);
        Assert.Equal(false, variables["a"]);
        Assert.Equal(true, variables["b"]);
        Assert.Equal(false, variables["c"]);
    }

    [Fact]
    public void Test_VisitNotExpr()
    {
        string code = @"
a = true;
b = not a;";
        var variables = RunEnv(code);
        Assert.Equal(false, variables["b"]);
    }
}
