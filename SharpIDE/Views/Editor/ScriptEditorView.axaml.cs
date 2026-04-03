using Avalonia;
using Avalonia.Controls;
using AvaloniaEdit;
using AvaloniaEdit.Indentation.CSharp;
using AvaloniaEdit.TextMate;
using TextMateSharp.Grammars;

namespace SharpIDE.Views.Editor;

public partial class ScriptEditorView : UserControl
{
    private TextMate.Installation? _textMateInstallation = null;

    public ScriptEditorView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        var editor = this.FindControl<TextEditor>("Editor");
        if (editor != null && _textMateInstallation == null)
        {
            var registryOptions = new SharpScriptRegistryOptions();
            _textMateInstallation = editor.InstallTextMate(registryOptions);
            _textMateInstallation.SetGrammar("source.svs");

            editor.Options.ConvertTabsToSpaces = true;
            editor.Options.IndentationSize = 4;
            editor.TextArea.IndentationStrategy = new CSharpIndentationStrategy(editor.Options);
        }
    }
}
