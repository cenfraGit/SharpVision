using AvaloniaEdit.Document;
using CommunityToolkit.Mvvm.ComponentModel;
using Dock.Model.Mvvm.Controls;

namespace SharpGUI.Panels;

public partial class ScriptEditorVM : Document
{
    [ObservableProperty] TextDocument _scriptDocument = new TextDocument("img = ReadImage(\"test.png\");");
}