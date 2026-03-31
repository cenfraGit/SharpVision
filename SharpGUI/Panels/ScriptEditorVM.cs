using AvaloniaEdit.Document;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Dock.Model.Mvvm.Controls;
using SharpGUI.Models.Messages;

namespace SharpGUI.Panels;

public partial class ScriptEditorVM : Document
{
    public string ScriptName { get; set; } = string.Empty;
    public string? ScriptPath { get; set; } = string.Empty;
    [ObservableProperty] TextDocument _scriptDocument;

    public ScriptEditorVM(string name, string? path, string contents)
    {
        this.Title = name;
        this.ScriptName = name;
        this.ScriptPath = path;
        this.ScriptDocument = new(contents);

        // whenever we receive a "run script" message, we'll broadcast back a message with the script data
        // before sending, save?
        WeakReferenceMessenger.Default.Register<MessageRunActiveScript>(this, (r, m) => {
            string scriptCode = ScriptDocument.Text;
            if (this.IsActive)
                WeakReferenceMessenger.Default.Send(new MessageExecuteCode(this.ScriptName, scriptCode));
        });
    }
}
