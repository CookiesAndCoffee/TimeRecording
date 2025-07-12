using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TimeRecording.Models;
using TimeRecording.Services;

namespace TimeRecording.ViewModels
{
    public class TimeRecordingViewModel
    {
        public ObservableCollection<Person> Persons { get; } = new();

        private TimeRecordingService _service;

        private Person _selectedPerson;
        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
                Load();
            }
        }

        private DateTime _selectedDate = DateTime.Today;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
                Load();
            }
        }

        private string _workingTime = "00:00";
        public string WorkingTime
        {
            get => _workingTime;
            set
            {
                if (TimeSpan.TryParse(value, out var ts))
                    _workingTime = ts.ToString(@"hh\:mm");
                else
                    _workingTime = "00:00";
                OnPropertyChanged();
            }
        }

        private int _targetTime = 0;
        public int TargetTime
        {
            get => _targetTime;
            set { _targetTime = value; OnPropertyChanged(); }
        }

        private int _balance = 0;
        public int Balance
        {
            get => _balance;
            set { _balance = value; OnPropertyChanged(); }
        }

        private int _monthlyTargetTime = 0;
        public int MonthlyTargetTime
        {
            get => _monthlyTargetTime;
            set { _monthlyTargetTime = value; OnPropertyChanged(); }
        }

        private int _monthlyBalance = 0;
        public int MonthlyBalance
        {
            get => _monthlyBalance;
            set { _monthlyBalance = value; OnPropertyChanged(); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public ICommand LoadingCommand { get; }
        public ICommand SavingCommand { get; }
        public ICommand ExportingCommand { get; }

        public TimeRecordingViewModel(TimeRecordingService service)
        {
            _service = service;
            LoadingCommand = new RelayCommand(Load, ButtonsEnabled);
            SavingCommand = new RelayCommand(Save, ButtonsEnabled);
            ExportingCommand = new RelayCommand(Export, ButtonsEnabled);
            LoadPersons();
        }

        public void LoadPersons()
        {
            Persons.Clear();
            var persons = _service.GetAllPersons();
            foreach (var person in persons)
                Persons.Add(person);
        }

        private void Load()
        {
            if (SelectedPerson == null || SelectedDate == default)
                return;
            var sollzeitZeiten = _service.GetTargetTimeModelTimesForDate(SelectedDate, SelectedPerson);
            TargetTime = sollzeitZeiten == null ? 0 : sollzeitZeiten.ForDate(SelectedDate);

            var saldo = _service.GetWorkingTime(SelectedDate, SelectedPerson)?.Minutes ?? 0;
            Balance = saldo - TargetTime;
            WorkingTime = TimeSpan.FromMinutes(saldo).ToString(@"hh\:mm");

            MonthlyTargetTime = _service.GetTargetTimeForMonth(SelectedDate, SelectedPerson);

            MonthlyBalance = _service.GetMonthlyBalance(SelectedDate, SelectedPerson);
        }

        private bool ButtonsEnabled()
        {
            return SelectedPerson != null && SelectedDate != default;
        }

        private void Save()
        {
            var minuten = 0;
            if (TimeSpan.TryParse(WorkingTime, out var ts))
                minuten = (int)ts.TotalMinutes;
            var arbeitszeit = new WorkingTime
            {
                Minutes = minuten,
                PersonId = SelectedPerson.Id,
                Date = SelectedDate
            };
            _service.SaveWorkingTime(arbeitszeit);
        }

        private async void Export()
        {
            try
            {
                IsBusy = true;
                await _service.ExportiereAlleZuCsvAsync();
                System.Windows.MessageBox.Show("Export erfolgreich!", "Export", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Fehler beim Export:\n{ex.Message}", "Export Fehler", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}