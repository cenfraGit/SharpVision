using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Dock.Model.Controls;
using Dock.Model.Core;
using SharpGUI.Models.Layout;
using SharpGUI.Models.Messages;
using SharpScript;
using SharpVision;

namespace SharpGUI.Views.Main;

public partial class MainWindowVM : ObservableObject
{
    public ObservableCollection<MenuItemViewModel> FunctionMenuItems { get; } = new();
    [ObservableProperty] IFactory? _factory;
    [ObservableProperty] IRootDock? _layout;

    public MainWindowVM()
    {
        Factory = new DockFactory();
        Layout = Factory?.CreateLayout();
        if (Layout != null)
        {
            Factory?.InitLayout(Layout);
        }

        // whenever we receive the requested script code, execute it
        WeakReferenceMessenger.Default.Register<MessageExecuteCode>(this, async (r, m) => {

            var environment = new SharpScriptEnvironment(m.ScriptCode);
            environment.Run();
            WeakReferenceMessenger.Default.Send(new MessageExecutionFinished(m.ScriptName, environment.Variables)); // send environment ref instead?
        });

        LoadFunctionsMenu();
    }

    [RelayCommand]
    private async Task OnButtonRun()
    {
        // send request to run active script
        WeakReferenceMessenger.Default.Send(new MessageRunActiveScript());
    }

    private void LoadFunctionsMenu()
    {
        var methods = typeof(Sharp).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

        var discoveredFunctions = methods
            .Select(m => new { Method = m, Attr = m.GetCustomAttribute<SharpFunctionAttribute>() })
            .Where(x => x.Attr != null)
            .ToList();

        var groupedFunctions = discoveredFunctions.GroupBy(x => x.Attr!.Group);

        foreach (var group in groupedFunctions)
        {
            var groupMenuItem = new MenuItemViewModel { Header = group.Key };

            foreach (var func in group)
            {
                var functionMenuItem = new MenuItemViewModel
                {
                    Header = func.Method.Name
                };

                groupMenuItem.Items.Add(functionMenuItem);
            }
            FunctionMenuItems.Add(groupMenuItem);
        }
    }
}

public class MenuItemViewModel
{
    public string Header { get; set; } = string.Empty;
    public ObservableCollection<MenuItemViewModel> Items { get; } = new();
}