using AvaloniaEdit.Document;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Dock.Model.Mvvm.Controls;
using SharpIDE.Models.Messages;

namespace SharpIDE.Views.Editor;

public partial class ScriptEditorVM : Document, IRecipient<MessageRunActiveScript>
{
    // --------------------------------------------------------------------------------
    // fields and properties
    // --------------------------------------------------------------------------------

    public string ScriptName { get; set; } = string.Empty;
    public string? ScriptPath { get; set; } = string.Empty;
    [ObservableProperty] TextDocument _scriptDocument;

    // --------------------------------------------------------------------------------
    // constructor
    // --------------------------------------------------------------------------------

    public ScriptEditorVM(string name, string? path, string contents)
    {
        this.Title = name;
        this.ScriptName = name;
        this.ScriptPath = path;
        this.ScriptDocument = new(contents);

        WeakReferenceMessenger.Default.Register(this);
    }

    // --------------------------------------------------------------------------------
    // methods
    // --------------------------------------------------------------------------------

    public async void Receive(MessageRunActiveScript m)
    {
        // whenever we receive a "run script" message, we'll broadcast back a message with the script data
        string scriptCode = ScriptDocument.Text;
        if (this.IsActive)
            WeakReferenceMessenger.Default.Send(new MessageExecuteCode(this.ScriptName, scriptCode));
    }
}
