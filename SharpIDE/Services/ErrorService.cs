using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using SharpScript;

namespace SharpIDE.Services;

public interface IErrorService
{
    ObservableCollection<SharpScriptException> Errors { get; set; }

    void AddError(SharpScriptException ex);
    void ClearErrors();
}

public class ErrorService : IErrorService
{
    public ObservableCollection<SharpScriptException> Errors { get; set; } = [];

    public void AddError(SharpScriptException ex)
    {
        this.Errors.Add(ex);
    }

    public void ClearErrors()
    {
        this.Errors.Clear();
    }
}
