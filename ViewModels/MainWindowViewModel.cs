using DynamicData;
using Hospital.Models;
using Hospital.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tmds.DBus.Protocol;

namespace Hospital.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public List<DrugDTO> Drugs { get; set; } = null!;


        private ObservableCollection<DrugDTO> _filteredDrugs = null!;

        public ObservableCollection<DrugDTO> FilteredDrugs
        {
            get { return _filteredDrugs; }
            set { _filteredDrugs = this.RaiseAndSetIfChanged(ref _filteredDrugs, value); }
        }

        private DrugDTO _selectedDrug = null!;

        public DrugDTO SelectedDrug
        {
            get { return _selectedDrug; }
            set { _selectedDrug = this.RaiseAndSetIfChanged(ref _selectedDrug, value); }
        }

        public List<Manufacturer> Manufacturers { get; set; } = new List<Manufacturer>()
        {
            new Manufacturer()
            {
                Id = 0,
                Name = "Все"
            }
        };

        private Manufacturer _selectedManufacturer = null!;

        public Manufacturer SelectedManufacturer
        {
            get { return _selectedManufacturer; }
            set { _selectedManufacturer = this.RaiseAndSetIfChanged(ref _selectedManufacturer, value); Filter(); }
        }

        public List<DrugProvider> DrugProviders { get; set; } = new List<DrugProvider>
        {
            new DrugProvider()
            {
                Id = 0,
                Name= "Все"
            }
        };

        private DrugProvider _selectedDrugProvider = null!;

        public DrugProvider SelectedDrugProvider
        {
            get { return _selectedDrugProvider; }
            set { _selectedDrugProvider = this.RaiseAndSetIfChanged(ref _selectedDrugProvider, value); Filter(); }
        }

        public List<DrugType> Types { get; set; } = new List<DrugType> { new DrugType() { Id = 0, Name = "Все" } };

        private DrugType _selectedType = null!;

        public DrugType SelectedType
        {
            get { return _selectedType; }
            set { _selectedType = this.RaiseAndSetIfChanged(ref _selectedType, value); Filter(); }
        }

        
        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value); }
        }

        private bool _isFilteredListNotNull;

        public bool IsFilteredListNotNull
        {
            get { return _isFilteredListNotNull; }
            set { _isFilteredListNotNull = this.RaiseAndSetIfChanged(ref _isFilteredListNotNull, value); }
        }


        public MainWindowViewModel()
        {
            GetContent();
        }

        private void GetContent()
        {
            Drugs = DBCall.GetDrugs();

            FilteredDrugs = new ObservableCollection<DrugDTO>(Drugs);
            SelectedDrug = FilteredDrugs[0];

            Manufacturers.AddRange(DBCall.GetManufacturers());
            SelectedManufacturer = Manufacturers[0];

            DrugProviders.AddRange(DBCall.GetDrugProviders());
            SelectedDrugProvider = DrugProviders[0];

            Types.AddRange(DBCall.GetTypes());
            SelectedType = Types[0];

        }

        private void Filter()
        {
            var filteredList = new List<DrugDTO>(Drugs);

            if (SelectedType != Types[0] && SelectedType != null)
            {
                filteredList = filteredList.Where(r => r.Type.Id == SelectedType.Id).ToList();
            }

            if (SelectedManufacturer != null && SelectedManufacturer != Manufacturers[0])
            {
                filteredList = filteredList.Where(r => r.Manufacturer.Id == SelectedManufacturer.Id).ToList();
            }

            if (SelectedDrugProvider != null && SelectedDrugProvider != DrugProviders[0])
            {
                filteredList = filteredList.Where(r => r.DrugProvider.Id == SelectedDrugProvider.Id).ToList();
            }

            FilteredDrugs.Clear();
            FilteredDrugs.AddRange(filteredList);

            if (FilteredDrugs.Any())
            {
                Message = "";
                IsFilteredListNotNull = true;
                SelectedDrug = FilteredDrugs[0];
            }
            else
            {
                IsFilteredListNotNull = false;
                Message = "Нет лекарств по выбранным категориям";
            }
        }
    }
}
