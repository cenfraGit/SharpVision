using System.Collections.Generic;

namespace SharpGUI.Models.Editor;

// used to "request" the active script's code
public class MessageRunActiveScript { }

// used to broadcast the script code
public class MessageExecuteCode
{
    public string Code { get; }
    public MessageExecuteCode(string code) => Code = code;
}

public class MessageExecutionFinished
{
    public string ScriptName { get; }
    public Dictionary<string, object?> ScriptVariables { get; }

    public MessageExecutionFinished(string scriptName, Dictionary<string, object?> scriptVariables)
    {
        this.ScriptName = scriptName;
        this.ScriptVariables = scriptVariables;
    }
}
