using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
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
    
    private void OpenReportWindow(object sender, RoutedEventArgs e)
    {
        var window = new ReportWindow();
        window.ShowDialog(this);
    }
    private void EditDrugCountWindow(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var dataContext = (MainWindowViewModel)button.DataContext;
        var SelectedDrug = dataContext.SelectedDrug;

        var window = new EditDrugCountWindow(SelectedDrug);
        window.ShowDialog(this);
    }
}