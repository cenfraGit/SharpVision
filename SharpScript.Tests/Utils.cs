using SharpScript;

namespace SharpScript.Tests;

internal class Utils
{
    // helper for getting environment variables
    public static Dictionary<string, object?> RunEnv(string code)
    {
        var env = new SharpScriptEnvironment(code);
        env.Run();
        return env.Variables;
    }
}
