using Dock.Model.Mvvm.Controls;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using SharpGUI.Models.Editor;

namespace SharpGUI.Panels;

public class VariableItem
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public partial class VariableExplorerVM : Tool
{
    public ObservableCollection<VariableItem> Variables { get; set; } = [];

    public VariableExplorerVM()
    {
        WeakReferenceMessenger.Default.Register<MessageExecutionFinished>(this, (r, m) => {
            Variables.Clear();
            foreach (var v in m.ScriptVariables)
            {
                Variables.Add(new VariableItem {
                        Name = v.Key,
                        Type = v.Value?.GetType().Name ?? "null",
                        Value = v.Value?.ToString() ?? "null"
                });
            }
        });
    }
}
