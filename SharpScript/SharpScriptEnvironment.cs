// --------------------------------------------------------------------------------
// SharpScriptEnvironment.cs
// Provides an environment for sharp script code execution.
// --------------------------------------------------------------------------------

using System.IO;
using System.Reflection;
using SharpVision;
using Antlr4.Runtime;

namespace SharpScript;

public class SharpScriptEnvironment
{
    // --------------------------------------------------------------------------------
    // fields and properties
    // --------------------------------------------------------------------------------

    // script variable table
    public Dictionary<string, object?> Variables { get; set; } = [];
    // for sharp methods
    public static Dictionary<string, MethodInfo> RegistryNative { get; } = [];
    // for user defined functions
    public Dictionary<string, SharpScriptParser.FunctionDeclarationStatContext> RegistryUser { get; } = [];

    // data related to script
    public Script Script { get; set; }

    private SharpScriptVisitor _visitor;
    public SharpScriptErrorListener ErrorListener { get; } = new();

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
            RegistryNative.Add(method.Name.Replace("2", ""), method);
        }
    }

    public SharpScriptEnvironment(Script script)
    {
        this.Script = script;
        // this could help user not type whole image paths?
        if (this.Script.Directory is not null && Directory.Exists(this.Script.Directory))
            Directory.SetCurrentDirectory(this.Script.Directory);
        this._visitor = new(this.Script, this.Variables, RegistryNative, this.RegistryUser);
    }

    // --------------------------------------------------------------------------------
    // methods
    // --------------------------------------------------------------------------------

    public void Reset()
    {
        this.Variables.Clear();
        this.RegistryUser.Clear();
        this._visitor = new(this.Script, this.Variables, RegistryNative, this.RegistryUser);
    }

    public void Run()
    {
        var inputStream = CharStreams.fromString(this.Script.Code);
        var lexer = new SharpScriptLexer(inputStream);
        var tokenStream = new CommonTokenStream(lexer);

        var parser = new SharpScriptParser(tokenStream);
        parser.RemoveErrorListeners();

        this.ErrorListener.Errors.Clear();
        parser.AddErrorListener(this.ErrorListener);

        var tree = parser.program();

        this._visitor.Visit(tree);
    }
}
