using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Threading.Tasks;
using Dock.Model.Controls;
using Dock.Model.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SharpGUI.Models.Layout;
using SharpScript;
using SharpGUI.Models.Editor;
using CommunityToolkit.Mvvm.Messaging;
using System.Text;

namespace SharpGUI.Views.Main;

public partial class MainWindowVM : ObservableObject
{
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

                var runner = new SharpScriptRunner(m.Code);
                runner.Run();

                StringBuilder sb = new();
                foreach (var variable in runner.Variables)
                    sb.Append($"{variable.Key}: {variable.Value}\n");

                var result = MessageBoxManager
                    .GetMessageBoxStandard("Script", $"Variables:\n{sb}", ButtonEnum.Ok);
                await result.ShowAsync();
            });
        }

        [RelayCommand]
        private async Task OnButtonRun()
        {
            // send request to run active script
            WeakReferenceMessenger.Default.Send(new MessageRunActiveScript());
        }
}
