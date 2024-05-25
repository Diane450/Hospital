using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Hospital.Models;
using Hospital.ViewModels;

namespace Hospital;

public partial class EditDrugCountWindow : Window
{
    public EditDrugCountWindow(DrugDTO drugDTO)
    {
        InitializeComponent();
        DataContext = new EditDrugCountWindowViewModel(drugDTO);
    }
}