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

        private int? _count = 1;

        public int? Count
        {
            get { return _count; }
            set
            {
                _count = this.RaiseAndSetIfChanged(ref _count, value);
            }
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
            //this.WhenAnyValue(x => x.Count).Subscribe(_ => IsEnabled());
        }
        public void Add()
        {
            try
            {
                if (Count > 0)
                {
                    DBCall.ReceiveDrug(CurrentDrug.Id, CurrentUser.Worker.Id, (int)Count);
                    CurrentDrug.Count += (int)Count;
                    Message = "Количество успешно изменено";
                    IsRemovingEnabled = CurrentDrug.Count > 0;
                }
                else
                {
                    Message = "Некорректное число";
                }
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
                if (Count > CurrentDrug.Count)
                {
                    Message = "Нельзя выдать больше лекарств, чем \n их есть на складе";
                }
                else if (Count > 0)
                {
                    DBCall.DispenseDrug(CurrentDrug.Id, CurrentUser.Worker.Id, (int)Count);
                    CurrentDrug.Count -= (int)Count;
                    Message = "Количество успешно изменено";
                    IsRemovingEnabled = CurrentDrug.Count > 0;
                }
                else if (Count <= 0)
                {
                    Message = "Некорректное число";
                }
            }
            catch
            {
                Message = "Ошибка соединения";
            }
        }
    }
}
