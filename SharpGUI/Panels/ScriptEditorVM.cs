using AvaloniaEdit.Document;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Dock.Model.Mvvm.Controls;
using SharpGUI.Models.Editor;

namespace SharpGUI.Panels;

public partial class ScriptEditorVM : Document
{
    [ObservableProperty] TextDocument _scriptDocument =
    new TextDocument("int x = 10;\nint y = x + 5;\nif (y - 15) {\n    print 999;\n}\nx = 2;\nint z = x * y;\nprint z;");


    public ScriptEditorVM()
    {
        // whenever we receive a "run script" message, we'll broadcast back a message with the script data
        WeakReferenceMessenger.Default.Register<MessageRunActiveScript>(this, (r, m) => {
            string scriptCode = ScriptDocument.Text;
            if (this.IsActive)
                WeakReferenceMessenger.Default.Send(new MessageExecuteCode(scriptCode));
        });
    }
}