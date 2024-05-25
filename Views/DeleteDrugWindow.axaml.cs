using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Hospital.Models;
using Hospital.ViewModels;

namespace Hospital;

public partial class DeleteDrugWindow : Window
{
    public DeleteDrugWindow(MainWindowViewModel model, DrugDTO drug)
    {
        InitializeComponent();
        DataContext = new DeleteDrugWindowViewModel(model, drug);
    }
    private void Close(object sender, RoutedEventArgs e)
    {
        Close();
    }
}