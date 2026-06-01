using SharpScript;

namespace SharpScript.Tests;

internal class Utils
{
    // helper for getting environment variables
    public static Dictionary<string, object?> RunEnv(string code)
    {
        var script = new Script("", "", code);
        var env = new SharpScriptEnvironment(script);
        env.Run();
        return env.Variables;
    }
}
