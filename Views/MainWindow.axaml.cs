using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Hospital.ViewModels;

namespace Hospital;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}