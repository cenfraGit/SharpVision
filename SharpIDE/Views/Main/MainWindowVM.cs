using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Dock.Model.Controls;
using Dock.Model.Core;
using SharpIDE.Models.Layout;
using SharpIDE.Models.Messages;
using SharpIDE.Views.Editor;
using SharpIDE.Services;
using SharpScript;
using SharpVision;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace SharpIDE.Views.Main;

public partial class MainWindowVM : ObservableObject, IRecipient<MessageExecuteCode>
{
    // --------------------------------------------------------------------------------
    // fields and properties
    // --------------------------------------------------------------------------------

    IDialogService _dialogService;
    public IErrorService ErrorService { get; set; }

    private static int _newFileCounter = 1;
    public ObservableCollection<MenuItemViewModel> FunctionMenuItems { get; } = new();
    [ObservableProperty] IFactory? _factory;
    [ObservableProperty] IRootDock? _layout;

    // --------------------------------------------------------------------------------
    // construtor
    // --------------------------------------------------------------------------------

    public MainWindowVM(IDialogService dialogService, IErrorService errorService)
    {
        this._dialogService = dialogService;
        this.ErrorService = errorService;

        Factory = new DockFactory();
        Layout = Factory?.CreateLayout();
        if (Layout != null)
            Factory?.InitLayout(Layout);

        WeakReferenceMessenger.Default.Register(this);

        LoadFunctionsMenu();
    }

    // --------------------------------------------------------------------------------
    // methods
    // --------------------------------------------------------------------------------

    public async void Receive(MessageExecuteCode m)
    {
        // whenever we receive the requested script code, execute it

        this.ErrorService.ClearErrors();

        var environment = new SharpScriptEnvironment(m.ScriptCode);
        bool isError = false;

        try
        {
            environment.Run();
        }
        catch (SharpScriptException sharpException)
        {
            this.ErrorService.AddError(sharpException);
            isError = true;
        }
        finally
        {
            foreach(var error in environment.ErrorListener.Errors)
            {
                this.ErrorService.AddError(error);
                isError = true; // care if reassign?
            }
        }

        if (!isError)
            WeakReferenceMessenger.Default.Send(new MessageExecutionFinished(m.ScriptName, environment));
    }

    private static IDocumentDock? GetScriptsDock(IDockable? root)
    {
        if (root == null) return null;
        if (root.Id == "Scripts" && root is IDocumentDock docDock) return docDock;

        if (root is IDock dock && dock.VisibleDockables != null)
        {
            foreach (var child in dock.VisibleDockables)
            {
                var found = GetScriptsDock(child);
                if (found != null) return found;
            }
        }
        return null;
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

    // --------------------------------------------------------------------------------
    // commands
    // --------------------------------------------------------------------------------

    [RelayCommand]
    private async Task OnButtonRun()
    {
        // send request to run active script
        WeakReferenceMessenger.Default.Send(new MessageRunActiveScript());
    }

    [RelayCommand]
    private async Task OnNewFile()
    {
        var scriptEditor = new ScriptEditorVM($"New{_newFileCounter++}", null, "");
        var documentDock = GetScriptsDock(Layout);

        if (documentDock != null)
        {
            documentDock.VisibleDockables ??= [];
            documentDock.VisibleDockables.Add(scriptEditor);
            documentDock.ActiveDockable = scriptEditor;
        }
    }

    [RelayCommand]
    private async Task OnOpenFile()
    {
        var files = await _dialogService.OpenFileAsync();
        if (files?.Count > 0)
        {
            var path = files[0].Path.LocalPath;
            var fileName = files[0].Name;

            var scriptEditor = new ScriptEditorVM(fileName, path, await File.ReadAllTextAsync(path));
            var documentDock = GetScriptsDock(Layout);

            if (documentDock != null)
            {
                documentDock.VisibleDockables ??= [];
                documentDock.VisibleDockables.Add(scriptEditor);
                documentDock.ActiveDockable = scriptEditor;
            }
        }
    }

    [RelayCommand]
    private async Task OnSaveFile()
    {
        var scriptsDock = GetScriptsDock(Layout);
        if (scriptsDock?.ActiveDockable is not ScriptEditorVM activeEditor)
            return;

        if (activeEditor.ScriptPath is null)
        {
            await OnSaveFileAs();
            return;
        }

        await File.WriteAllTextAsync(activeEditor.ScriptPath, activeEditor.ScriptDocument.Text);
    }

    [RelayCommand]
    private async Task OnSaveFileAs()
    {
        var scriptsDock = GetScriptsDock(Layout);
        if (scriptsDock?.ActiveDockable is not ScriptEditorVM activeEditor)
            return;

        var file = await _dialogService.SaveFileAsync(activeEditor.Title ?? "new_script.svs");

        if (file != null)
        {
            try
            {
                var path = file.Path.LocalPath;
                await File.WriteAllTextAsync(path, activeEditor.ScriptDocument.Text);

                activeEditor.Title = file.Name;
                activeEditor.ScriptPath = path;
                activeEditor.Id = path;
            }
            catch (Exception)
            {
                // ...?
            }
        }
    }
}

public class MenuItemViewModel
{
    public string Header { get; set; } = string.Empty;
    public ObservableCollection<MenuItemViewModel> Items { get; } = new();
}
