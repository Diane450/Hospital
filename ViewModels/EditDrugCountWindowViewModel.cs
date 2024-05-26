using Hospital.Models;
using Hospital.ModelsDTO;
using Hospital.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public class EditDrugCountWindowViewModel : ViewModelBase
    {
        public DrugDTO CurrentDrug { get; set; }

        public string Info { get; set; } = "Введите количество выписываемого \n или получаемого препарата(уп.)";
        
        private int _count;

        public int Count
        {
            get { return _count; }
            set { _count = this.RaiseAndSetIfChanged(ref _count, value); }
        }

        private string _message = null!;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value); }
        }

        private bool _isRemovingEnabled;

        public bool IsRemovingEnabled
        {
            get { return _isRemovingEnabled; }
            set { _isRemovingEnabled = this.RaiseAndSetIfChanged(ref _isRemovingEnabled, value); }
        }

        public EditDrugCountWindowViewModel(DrugDTO drugDTO)
        {
            CurrentDrug = drugDTO;
            IsRemovingEnabled = CurrentDrug.Count > 0;
        }

        public void Add()
        {
            try
            {
                DBCall.ReceiveDrug(CurrentDrug.Id, CurrentUser.Worker.Id, Count);
                CurrentDrug.Count += Count;
                Message = "Количество успешно изменено";
                IsRemovingEnabled = CurrentDrug.Count > 0;
            }
            catch
            {
                Message = "Ошибка соединения";
            }
        }

        public void Remove()
        {
            try
            {
                if(Count > CurrentDrug.Count)
                {
                    Message = "Нельзя выдать больше лекарств, чем \n их есть на складе";
                }
                else
                {
                    DBCall.DispenseDrug(CurrentDrug.Id, CurrentUser.Worker.Id, Count);
                    CurrentDrug.Count -= Count;
                    Message = "Количество успешно изменено";
                    IsRemovingEnabled = CurrentDrug.Count > 0;
                }
            }
            catch
            {
                Message = "Ошибка соединения";
            }
        }
    }
}
