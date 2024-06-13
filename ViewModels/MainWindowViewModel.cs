using Avalonia.Controls.Documents;
using DynamicData;
using Hospital.Models;
using Hospital.ModelsDTO;
using Hospital.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

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

        public List<Manufacturer> Manufacturers { get; set; } =
        [
            new Manufacturer()
            {
                Id = 0,
                Name = "Все"
            }
        ];

        private Manufacturer _selectedManufacturer = null!;

        public Manufacturer SelectedManufacturer
        {
            get { return _selectedManufacturer; }
            set { _selectedManufacturer = this.RaiseAndSetIfChanged(ref _selectedManufacturer, value); Filter(); }
        }

        public List<DrugProvider> DrugProviders { get; set; } =
        [
            new()
            {
                Id = 0,
                Name= "Все"
            }
        ];

        private DrugProvider _selectedDrugProvider = null!;

        public DrugProvider SelectedDrugProvider
        {
            get { return _selectedDrugProvider; }
            set { _selectedDrugProvider = this.RaiseAndSetIfChanged(ref _selectedDrugProvider, value); Filter(); }
        }

        public List<DrugType> Types { get; set; } = [new DrugType() { Id = 0, Name = "Все" }];

        private DrugType _selectedType = null!;

        public DrugType SelectedType
        {
            get { return _selectedType; }
            set { _selectedType = this.RaiseAndSetIfChanged(ref _selectedType, value); Filter(); }
        }


        private string _message = null!;

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


        private string _searchingDrug;

        public string SearchingDrug
        {
            get { return _searchingDrug; }
            set { _searchingDrug = this.RaiseAndSetIfChanged(ref _searchingDrug, value); }
        }

        public List<string> SortValues { get; set; }

        private string _selectedSortValue;

        public string SelectedSortValue
        {
            get { return _selectedSortValue; }
            set { _selectedSortValue = this.RaiseAndSetIfChanged(ref _selectedSortValue, value); Sort(); }
        }

        public bool IsAdmin { get; set; }
        public MainWindowViewModel()
        {
            GetContent();
            this.WhenAnyValue(x => x.SearchingDrug).Subscribe(_ => Find());
            IsAdmin = CurrentUser.Worker.User.RoleId == 1;
        }

        private void Find()
        {
            if (!string.IsNullOrEmpty(SearchingDrug))
            {
                FilteredDrugs = new ObservableCollection<DrugDTO>(Drugs.Where(d => d.Name.ToLower().StartsWith(SearchingDrug.ToLower())).ToList());
                if (FilteredDrugs.Count == 0 || FilteredDrugs == null)
                {
                    IsFilteredListNotNull = false;
                    Message = "Не найдено";
                }
                else
                { 
                    IsFilteredListNotNull = true;
                    Message = "";
                    SelectedDrug = FilteredDrugs[0];
                }
            }
            else
            {
                Filter();
            }
        }
        
        private void GetContent()
        {
            Drugs = DBCall.GetDrugs();

            FilteredDrugs = new ObservableCollection<DrugDTO>(Drugs);
            SelectedDrug = FilteredDrugs[0];

            SortValues =
            [
                "от А до Я",
                "от Я до А",
                "количество: от меньшего к большему",
                "количество: от большего к меньшему",
            ];

            Manufacturers.AddRange(DBCall.GetManufacturers());
            SelectedManufacturer = Manufacturers[0];

            DrugProviders.AddRange(DBCall.GetDrugProviders());
            SelectedDrugProvider = DrugProviders[0];

            Types.AddRange(DBCall.GetTypes());
            SelectedType = Types[0];

            SelectedSortValue = SortValues[0];
        }

        public void Filter()
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
                Sort();
                Message = "";
                IsFilteredListNotNull = true;
            }
            else
            {
                IsFilteredListNotNull = false;
                Message = "Не найдено";
            }
        }
    
        private void Sort()
        {
            if (SelectedSortValue == SortValues[0])
            {
                FilteredDrugs = new ObservableCollection<DrugDTO>([.. FilteredDrugs.OrderBy(d => d.Name)]);
            }
            else if (SelectedSortValue == SortValues[1])
            {
                FilteredDrugs = new ObservableCollection<DrugDTO>([.. FilteredDrugs.OrderByDescending(d => d.Name)]);
            }
            else if (SelectedSortValue == SortValues[2])
            {
                FilteredDrugs = new ObservableCollection<DrugDTO>([.. FilteredDrugs.OrderBy(d => d.Count)]);
            }
            else
            {
                FilteredDrugs = new ObservableCollection<DrugDTO>([.. FilteredDrugs.OrderByDescending(d => d.Count)]);
            }
            SelectedDrug = FilteredDrugs[0];
        }
    }
}
