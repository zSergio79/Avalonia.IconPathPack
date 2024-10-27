using Avalonia.Controls;
using Avalonia.IconPathPack.ViewModels;
using Avalonia.ReactiveUI;

using ReactiveUI;

namespace Avalonia.IconPathPack.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WhenActivated(d => { });
        }
    }
}