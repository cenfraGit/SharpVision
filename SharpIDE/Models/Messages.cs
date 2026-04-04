using SharpScript;
using System.Collections.Generic;

namespace SharpIDE.Models.Messages;

// used to "request" the active script's code
public class MessageRunActiveScript { }

// used to broadcast the script code
public record MessageExecute(Script Script);

public class MessageExecutionFinished
{
    public string ScriptName { get; }
    public SharpScriptEnvironment Environment { get; }

    public MessageExecutionFinished(string scriptName, SharpScriptEnvironment environment)
    {
        this.ScriptName = scriptName;
        this.Environment = environment;
    }
}

public class MessageVariableSelected
{
    public string Name { get; }
    public object? Value { get; }

    public MessageVariableSelected(string name, object? value)
    {
        this.Name = name;
        this.Value = value;
    }
}

public enum EditorAction { Cut, Copy, Paste, Undo, Redo, AddLineComment, RemoveLineComment }
public record MessageEditorAction(EditorAction Action);
