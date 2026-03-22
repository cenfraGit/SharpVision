using Dock.Model.Controls;
using Dock.Model.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using SharpGUI.Models.Layout;

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
        }
}