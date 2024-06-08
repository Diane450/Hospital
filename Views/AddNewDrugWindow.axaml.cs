using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Hospital.Models;
using Hospital.ViewModels;
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
        var context = (AddNewDrugWindowViewModel)button.DataContext!;

        var storageProvider = StorageProvider;
        var result = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Выбрать изображение",
            FileTypeFilter =
            [
                new FilePickerFileType("Images")
                {
                    Patterns = ["*.jpg", "*.png", "*.jpeg"]
                }
            ],
            AllowMultiple = false
        });

        if (result.Count > 0)
        {
            var selectedFile = result[0];
            await using var fs = await selectedFile.OpenReadAsync();
            var bitmap = new Bitmap(fs);

            using var ms = new MemoryStream();
            bitmap.Save(ms);
            context.Drug.Photo = ms.ToArray();
        }
    }
}