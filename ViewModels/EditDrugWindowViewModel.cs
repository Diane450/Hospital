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
    public class EditDrugWindowViewModel : ViewModelBase
    {
        public DrugDTO CurrentDrug { get; set; }

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

        private int? _count;

        public int? Count
        {
            get { return _count; }
            set { _count = this.RaiseAndSetIfChanged(ref _count, value); }
        }

        public MainWindowViewModel Model { get; set; } = null!;

        private string _message = null!;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value); }
        }
        public EditDrugWindowViewModel(DrugDTO drug)
        {
            Drug = new DrugDTO
            {
                Id = drug.Id,
                Name = drug.Name,
                Photo = drug.Photo
            };

            Count = drug.Count;
            CurrentDrug = drug;

            Manufacturers = DBCall.GetManufacturers();
            SelectedManufacturer = Manufacturers.First(m => m.Id == CurrentDrug.Manufacturer.Id);

            DrugProviders = DBCall.GetDrugProviders();
            SelectedDrugProvider = DrugProviders.First(p => p.Id == CurrentDrug.DrugProvider.Id);

            Types = DBCall.GetTypes();
            SelectedType = Types.First(p => p.Id == CurrentDrug.Type.Id);

            this.WhenAnyValue(
                x => x.Drug.Name,
                x => x.Drug.Photo,
                x => x.Count).Subscribe(_ => IsButtonEnabled());
        }

        private void IsButtonEnabled()
        {
            IsButtonEnable = !string.IsNullOrEmpty(Drug.Name) && Drug.Photo != null && Count != null;
        }

        public void Edit()
        {
            try
            {
                Drug.Manufacturer = SelectedManufacturer;
                Drug.DrugProvider = SelectedDrugProvider;
                Drug.Type = SelectedType;
                Drug.Count = (int)Count!;
                DBCall.EditDrug(Drug);

                if (CurrentDrug.Count > Drug.Count)
                    DBCall.DispenseDrug(Drug.Id, CurrentUser.Worker.Id, CurrentDrug.Count - Drug.Count);
                else if (CurrentDrug.Count < Drug.Count)
                    DBCall.ReceiveDrug(Drug.Id, CurrentUser.Worker.Id, Drug.Count - CurrentDrug.Count);

                CurrentDrug.Name = Drug.Name;
                CurrentDrug.Photo = Drug.Photo;
                CurrentDrug.Type = Drug.Type;
                CurrentDrug.Manufacturer = Drug.Manufacturer;
                CurrentDrug.DrugProvider = Drug.DrugProvider;
                CurrentDrug.Count = Drug.Count;

                Message = "Данные обновлены";
            }
            catch
            {
                Message = "Не удалось обновить данные";
            }
        }
    }
}
