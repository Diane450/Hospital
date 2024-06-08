using Hospital.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public class ReportWindowViewModel(ReportWindow window) : ViewModelBase
    {
        public DateTime Today { get; set; } = DateTime.Now;

        private DateTime? _selectedDateStart = DateTime.Now;

        public DateTime? SelectedDateStart
        {
            get { return _selectedDateStart; }
            set
            {
                _selectedDateStart = this.RaiseAndSetIfChanged(ref _selectedDateStart, value);
            }
        }

        private DateTime? _selectedDateEnd = DateTime.Now;

        public DateTime? SelectedDateEnd
        {
            get { return _selectedDateEnd; }
            set
            {
                _selectedDateEnd = this.RaiseAndSetIfChanged(ref _selectedDateEnd, value);
            }
        }

        private string _message = null!;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value); }
        }

        public ReportWindow ReportWindow { get; set; } = window;

        public async Task CreateReport()
        {
            try
            {
                DateTime DateStart = (DateTime)SelectedDateStart!;
                DateTime DateEnd = (DateTime)SelectedDateEnd!;

                DateOnly[] range =
                [
                    new(DateStart.Year, DateStart.Month, DateStart.Day),
                    new(DateEnd.Year, DateEnd.Month, DateEnd.Day)
                ];
                Array.Sort(range);
                Report report = new(range);
                report.GetReportData();
                await report.CreateReport(ReportWindow);
                Message = "Отчет готов";
            }
            catch
            {
                Message = "Ошибка соединения";
            }
        }
    }
}
