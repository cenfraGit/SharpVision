using Avalonia;
using Avalonia.Controls;
using AvaloniaEdit;
using AvaloniaEdit.Indentation.CSharp;
using AvaloniaEdit.TextMate;
using TextMateSharp.Grammars;

namespace SharpGUI.Views.Editor;

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

            editor.TextArea.TextEntering += (s, e) =>
            {
                if (e.Text == "{")
                {
                    editor.TextArea.Selection.ReplaceSelectionWithText("{}");
                    editor.CaretOffset--;
                    e.Handled = true;
                }
                else if (e.Text == "\"")
                {
                    editor.TextArea.Selection.ReplaceSelectionWithText("\"\"");
                    editor.CaretOffset--;
                    e.Handled = true;
                }
            };
        }
        
        // var textEditor = this.FindControl<TextEditor>("Editor");
        // var registryOptions = new RegistryOptions(ThemeName.OneDark);
        // var textMateInstallation = textEditor.InstallTextMate(registryOptions);
        // textMateInstallation.SetGrammar(
        //     registryOptions.GetScopeByLanguageId(
        //         registryOptions.GetLanguageByExtension(".cs").Id));

    }
}