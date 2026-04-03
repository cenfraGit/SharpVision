using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using SharpIDE.Views.Main;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using SharpIDE.Services;
using System;

namespace SharpIDE;

public partial class App : Application
{
    public static IServiceProvider? ServiceProvider { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        IServiceCollection collection = new ServiceCollection();

        collection.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
        collection.AddSingleton<IDialogService, DialogService>();
        collection.AddTransient<IErrorService, ErrorService>();

        collection.AddTransient<MainWindowVM>();

        ServiceProvider = collection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            var mainWindow = new MainWindow();
            var dialogService = ServiceProvider.GetRequiredService<IDialogService>();
            dialogService.Initialize(mainWindow);
            mainWindow.DataContext = ServiceProvider.GetRequiredService<MainWindowVM>();
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
