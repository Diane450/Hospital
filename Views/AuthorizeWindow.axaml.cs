using Avalonia.Controls;
using Avalonia.Interactivity;
using Hospital.Models;
using Hospital.ModelsDTO;
using Hospital.Services;
using System;

namespace Hospital.Views
{
    public partial class AuthorizeWindow : Window
    {
        public AuthorizeWindow()
        {
            InitializeComponent();
        }
        private void Authorize(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox code = this.Find<TextBox>("Code")!;
                TextBox login = this.Find<TextBox>("Login")!;
                Worker worker = DBCall.Authorize(login.Text!, code.Text!);
                if (worker != null)
                {
                    CurrentUser.Worker = worker;
                    var window = new MainWindow();
                    window.Show();
                    Close();
                }
                else
                {
                    Label ErrorLabel = this.Find<Label>("ErrorLabel")!;
                    ErrorLabel.IsVisible = true;
                    ErrorLabel.Content = "Неправильный код";
                }
            }
            catch (Exception ex)
            {
                Label ErrorLabel = this.Find<Label>("ErrorLabel")!;
                ErrorLabel.IsVisible = true;
                ErrorLabel.Content = "Ошибка соединения";
            }
        }
    }
}