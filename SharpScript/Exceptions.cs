using System;
using System.Collections.Generic;

namespace SharpScript;

public class SharpScriptException : Exception
{
    public string? Line { get; set; } = null;
    public string? Column { get; set; } = null;
    public string ExMessage { get => this.Message; }

    public SharpScriptException(string message) : base(message)
    {
    }

    public SharpScriptException(string message, dynamic context) : base(message)
    {
        this.Line = context.Start.Line.ToString();
        this.Column = context.Start.Column.ToString();;
    }

    public SharpScriptException(string message, int line, int column) : base(message)
    {
        this.Line = line.ToString();
        this.Column = column.ToString();
    }
}
