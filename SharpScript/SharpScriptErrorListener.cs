using Antlr4.Runtime;
using System.Collections.Generic;

namespace SharpScript;

public class SharpScriptErrorListener : BaseErrorListener
{
    public List<SharpScriptException> Errors { get; } = [];

    public override void SyntaxError(TextWriter output,
                                     IRecognizer recognizer,
                                     IToken offendingSymbol,
                                     int line,
                                     int charPositionInLine,
                                     string msg,
                                     RecognitionException e)
    {
        this.Errors.Add(new SharpScriptException($"Syntax error: {msg}",
                                                 line,
                                                 charPositionInLine));
    }
}
