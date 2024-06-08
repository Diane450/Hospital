using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Hospital.ViewModels;
using Org.BouncyCastle.Asn1.Crmf;

namespace Hospital;

public partial class ReportWindow : Window
{
    public ReportWindow()
    {
        InitializeComponent();
        DataContext = new ReportWindowViewModel(this);
    }
}