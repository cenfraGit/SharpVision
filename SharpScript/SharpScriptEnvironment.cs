// --------------------------------------------------------------------------------
// SharpScriptEnvironment.cs
// Provides an environment for sharp script code execution.
// --------------------------------------------------------------------------------

using System.Reflection;
using SharpVision;
using Antlr4.Runtime;

namespace SharpScript;

public class SharpScriptEnvironment
{
    public string Code { get; set; } = string.Empty;

    private SharpScriptVisitor _visitor;

    public Dictionary<string, object?> Variables { get; set; } = [];
    // for sharp methods
    public static Dictionary<string, MethodInfo> RegistryNative { get; } = [];
    // for user defined functions
    public Dictionary<string, SharpScriptParser.FunctionDeclarationStatContext> RegistryUser { get; } = [];

    // --------------------------------------------------------------------------------
    // constructors
    // --------------------------------------------------------------------------------

    // discovers and loads sharp methods via reflection
    static SharpScriptEnvironment()
    {
        Type targetType = typeof(Sharp);
        MethodInfo[] methods = targetType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
        foreach (MethodInfo method in methods)
        {
            var attribute = method.GetCustomAttribute<SharpFunctionAttribute>();
            if (attribute is null) continue;
            RegistryNative.Add(method.Name, method);
        }
    }

    public SharpScriptEnvironment()
    {
        this._visitor = new(this.Variables, RegistryNative, this.RegistryUser);
    }

    public SharpScriptEnvironment(string code)
    {
        this.Code = code;
        this._visitor = new(this.Variables, RegistryNative, this.RegistryUser);
    }

    // --------------------------------------------------------------------------------
    // methods
    // --------------------------------------------------------------------------------

    public void Reset()
    {
        this.Variables.Clear();
        this.RegistryUser.Clear();
        this._visitor = new(this.Variables, RegistryNative, this.RegistryUser);
    }

    public void Run()
    {
        var inputStream = CharStreams.fromString(this.Code);
        var lexer = new SharpScriptLexer(inputStream);
        var tokenStream = new CommonTokenStream(lexer);

        var parser = new SharpScriptParser(tokenStream);
        var tree = parser.program();

        this._visitor.Visit(tree);
    }
}