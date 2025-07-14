using System.Collections.ObjectModel;
using System.Windows.Input;
using TimeRecording.Models;
using TimeRecording.Services.Interfaces;

namespace TimeRecording.ViewModels
{
    public class TimeRecordingViewModel : AbstractViewModel<WorkingTime, IWorkingTimeService>
    {
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

        public ObservableCollection<Person> Persons { get; } = new();

        private IPersonService _personService;

        public TimeRecordingViewModel(IWorkingTimeService service, IPersonService personService) : base(service)
        {
            _personService = personService;
            LoadPersons();
        }

        protected override bool CanSave()
        {
            return SelectedDate != default && !String.IsNullOrWhiteSpace(WorkingTime);
        }

        public void LoadPersons()
        {
            AddToCollection(_personService, Persons);
        }

        protected override void Load()
        {
            if (SelectedPerson == null || SelectedDate == default)
                return;

            var sollzeitZeiten = _personService.GetTargetTimeModelTimesForDate(SelectedPerson, SelectedDate);
            TargetTime = sollzeitZeiten == null ? 0 : sollzeitZeiten.ForDate(SelectedDate);

            SelectedEntity = _service.GetWorkingTimeForDate(SelectedPerson, SelectedDate) ?? new WorkingTime();
            var saldo = SelectedEntity?.Minutes ?? 0;
            Balance = saldo - TargetTime;
            WorkingTime = TimeSpan.FromMinutes(saldo).ToString(@"hh\:mm");

            MonthlyTargetTime = _personService.GetTargetTimeForMonth(SelectedPerson, SelectedDate);

            MonthlyBalance = _personService.GetMonthlyBalance(SelectedPerson, SelectedDate);
        }

        protected override void PreSave()
        {
            var minuten = 0;
            if (TimeSpan.TryParse(WorkingTime, out var ts))
                minuten = (int)ts.TotalMinutes;
            SelectedEntity.Minutes = minuten;
            SelectedEntity.PersonId = SelectedPerson.Id;
            SelectedEntity.Date = SelectedDate;
        }
    }
}