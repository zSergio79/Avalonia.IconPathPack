using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.IconPathPack.ViewModels;
using Avalonia.IconPathPack.Views;
using Avalonia.Markup.Xaml;

namespace Avalonia.IconPathPack
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                desktop.MainWindow.DataContext = new MainWindowViewModel(desktop.MainWindow.Clipboard);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}