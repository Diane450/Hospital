using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Hospital.ViewModels;
using Hospital.Views;

namespace Hospital
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
                desktop.MainWindow = new AuthorizeWindow
                {
                    DataContext = new AuthorizeWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}