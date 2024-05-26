using Hospital.Models;
using Hospital.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public class AddNewDrugWindowViewModel : ViewModelBase
    {
        private DrugDTO _drug = null!;

        public DrugDTO Drug
        {
            get { return _drug; }
            set { _drug = this.RaiseAndSetIfChanged(ref _drug, value); }
        }

        private bool _isButtonEnable;
        public bool IsButtonEnable
        {
            get { return _isButtonEnable; }
            set { _isButtonEnable = this.RaiseAndSetIfChanged(ref _isButtonEnable, value); }
        }

        public List<Manufacturer> Manufacturers { get; set; }

        private Manufacturer _selectedManufacturer = null!;

        public Manufacturer SelectedManufacturer
        {
            get { return _selectedManufacturer; }
            set { _selectedManufacturer = this.RaiseAndSetIfChanged(ref _selectedManufacturer, value); }
        }

        public List<DrugProvider> DrugProviders { get; set; }


        private DrugProvider _selectedDrugProvider = null!;

        public DrugProvider SelectedDrugProvider
        {
            get { return _selectedDrugProvider; }
            set { _selectedDrugProvider = this.RaiseAndSetIfChanged(ref _selectedDrugProvider, value); }
        }

        public List<DrugType> Types { get; set; }

        private DrugType _selectedType = null!;

        public DrugType SelectedType
        {
            get { return _selectedType; }
            set { _selectedType = this.RaiseAndSetIfChanged(ref _selectedType, value); }
        }


        public MainWindowViewModel Model { get; set; }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value); }
        }

        private int? _count;

        public int? Count
        {
            get { return _count; }
            set { _count = this.RaiseAndSetIfChanged(ref _count, value); }
        }
        public AddNewDrugWindowViewModel(MainWindowViewModel model, DrugDTO drug)
        {
            Drug = new DrugDTO();
            Model = model;
            Manufacturers = DBCall.GetManufacturers();
            SelectedManufacturer = Manufacturers[0];

            DrugProviders = DBCall.GetDrugProviders();
            SelectedDrugProvider = DrugProviders[0];

            Types = DBCall.GetTypes();
            SelectedType = Types[0];

            this.WhenAnyValue(
                x => x.Drug.Name,
                x => x.Drug.Photo,
                x => x.Count).Subscribe(_ => IsButtonEnabled());
        }

        private void IsButtonEnabled()
        {
            IsButtonEnable = !string.IsNullOrEmpty(Drug.Name) && Drug.Photo != null && Count != null;
        }

        public void Add()
        {
            try
            {
                Drug.Manufacturer = SelectedManufacturer;
                Drug.Count = (int)Count!;
                Drug.DrugProvider = SelectedDrugProvider;
                Drug.Type = SelectedType;
                Drug.Id = DBCall.AddDrug(Drug);
                Model.Drugs.Add(Drug);
                Model.Filter();
                Message = "Новый препарат добавлен";
            }
            catch
            {
                Message = "Не удалось добавить новый препарат";
            }
        }
    }
}
