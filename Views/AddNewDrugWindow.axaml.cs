using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Hospital.Models;
using Hospital.ViewModels;
using System.Drawing;
using System.IO;

namespace Hospital;

public partial class AddNewDrugWindow : Window
{
    public AddNewDrugWindow(MainWindowViewModel model, DrugDTO drug)
    {
        InitializeComponent();
        DataContext = new AddNewDrugWindowViewModel(model, drug);
    }

    private async void ChangePhoto(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var context = (AddNewDrugWindowViewModel)button.DataContext;

        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filters.Add(new FileDialogFilter() { Name = "Images", Extensions = { "jpg", "png", "jpeg" } });

        string[] result = await dialog.ShowAsync(this);

        if (result != null && result.Length > 0)
        {
            using (FileStream fs = File.OpenRead(result[0]))
            {
                Avalonia.Media.Imaging.Bitmap bp = new Avalonia.Media.Imaging.Bitmap(fs);
                
                using (MemoryStream ms = new MemoryStream())
                {
                    bp.Save(ms);
                    context.Drug.Photo = ms.ToArray();
                }
            }
        }
    }
}