namespace SharpGUI.Models.Editor;

// used to "request" the active script's code
public class MessageRunActiveScript { }

// used to broadcast the script code
public class MessageExecuteCode
{
    public string Code { get; }
    public MessageExecuteCode(string code) => Code = code;
}
