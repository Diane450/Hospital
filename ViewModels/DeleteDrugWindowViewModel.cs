using Hospital.Models;
using Hospital.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public class DeleteDrugWindowViewModel(MainWindowViewModel model, DrugDTO drug) : ViewModelBase
    {
        public MainWindowViewModel Model { get; set; } = model;

        public DrugDTO Drug { get; set; } = drug;


        private string _message = null!;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value); }
        }

        private bool _isButtonEnable = true;

        public bool IsButtonEnable
        {
            get { return _isButtonEnable; }
            set { _isButtonEnable = this.RaiseAndSetIfChanged(ref _isButtonEnable, value); }
        }

        public void Delete()
        {
            try
            {
                DBCall.DeleteDrug(Drug);
                Model.Drugs.Remove(Drug);
                Model.Filter();
                Message = "Успешно удалено";
                IsButtonEnable = false;
            }
            catch (Exception)
            {
                Message = "Не удалось удалить препарат";
            }
        }
    }
}
