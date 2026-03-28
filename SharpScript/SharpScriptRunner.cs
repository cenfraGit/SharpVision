using System;
using Antlr4.Runtime;

namespace SharpScript;

public class SharpScriptRunner
{
    public static void Run(string code)
    {
        var inputStream = CharStreams.fromString(code);
        var lexer = new SharpScriptLexer(inputStream);
        var tokenStream = new CommonTokenStream(lexer);

        var parser = new SharpScriptParser(tokenStream);
        var tree = parser.program();

        var visitor = new SharpScriptVisitor();
        visitor.Visit(tree);
    }
}