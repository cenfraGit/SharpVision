using System;
using Antlr4.Runtime;

namespace SharpScript;

// environment for script execution
public class SharpScriptRunner
{
    public string Code { get; set; } = string.Empty;
    public Dictionary<string, int> Variables { get; set; } = [];

    public SharpScriptRunner()
    {
    }

    public SharpScriptRunner(string code)
    {
        this.Code = code;
    }

    public void Run()
    {
        var inputStream = CharStreams.fromString(this.Code);
        var lexer = new SharpScriptLexer(inputStream);
        var tokenStream = new CommonTokenStream(lexer);

        var parser = new SharpScriptParser(tokenStream);
        var tree = parser.program();

        var visitor = new SharpScriptVisitor();
        visitor.Visit(tree);

        // extract variables from visitor
        this.Variables = visitor.Variables;
    }
}