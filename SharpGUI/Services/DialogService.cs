using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace SharpGUI.Services;

public interface IDialogService
{
    void Initialize(Window window);
    Task<IReadOnlyList<IStorageFile>?> OpenFileAsync();
    Task<IStorageFile?> SaveFileAsync(string suggestedName = "script.svs");
}

public class DialogService : IDialogService
{
    private Window? _mainWindow;

    public void Initialize(Window window) => _mainWindow = window;

    public async Task<IReadOnlyList<IStorageFile>?> OpenFileAsync()
    {
        if (_mainWindow == null) 
            throw new InvalidOperationException("DialogService: window was null");

        return await _mainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open File",
            AllowMultiple = false,
            FileTypeFilter = [SvsFileType]
        });
    }

    public async Task<IStorageFile?> SaveFileAsync(string suggestedName = "script.svs")
    {
        if (_mainWindow == null) 
            throw new InvalidOperationException("DialogService: window was null");

        return await _mainWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Script As",
            SuggestedFileName = suggestedName,
            DefaultExtension = ".svs",
            FileTypeChoices = [SvsFileType]
        });
    }

    private static FilePickerFileType SvsFileType => new("SharpScript Files")
    {
        Patterns = ["*.svs"],
        AppleUniformTypeIdentifiers = ["public.text"],
        MimeTypes = ["text/plain"]
    };
}