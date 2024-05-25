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
    public class DeleteDrugWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel Model { get; set; }

        public DrugDTO Drug { get; set; }


        private string _message = null!;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value); }
        }

        public DeleteDrugWindowViewModel(MainWindowViewModel model, DrugDTO drug)
        {
            Model = model;
            Drug = drug;
        }

        public void Delete()
        {
            try
            {
                DBCall.DeleteDrug(Drug);
                Model.Drugs.Remove(Drug);
                Model.Filter();
                Message = "Успешно удалено";
            }
            catch (Exception)
            {
                Message = "Не удалось удалить препарат";
            }
        }
    }
}
