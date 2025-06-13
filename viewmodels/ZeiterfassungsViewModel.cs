using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Zeiterfassung.models;
using Zeiterfassung.services;
using Zeiterfassung.views;

namespace Zeiterfassung.viewmodels
{
    public class ZeiterfassungsViewModel : INotifyPropertyChanged
    {
        public ZeiterfassungsView View { get; set; }

        public ObservableCollection<Person> Personen { get; } = new();

        private ZeiterfassungsService _service { get; } = new ZeiterfassungsService(ZeiterfassungsDBContext.Instance);

        private Person _selectedPerson;
        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
                Laden(null);
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
                Laden(null);
            }
        }

        private string _arbeitszeit = "00:00";
        public string Arbeitszeit
        {
            get => _arbeitszeit;
            set
            {
                if (TimeSpan.TryParse(value, out var ts))
                    _arbeitszeit = ts.ToString(@"hh\:mm");
                else
                    _arbeitszeit = "00:00";
                OnPropertyChanged();
            }
        }

        private int _sollzeit = 0;
        public int Sollzeit
        {
            get => _sollzeit;
            set { _sollzeit = value; OnPropertyChanged(); }
        }

        private int _saldo = 0;
        public int Saldo
        {
            get => _saldo;
            set { _saldo = value; OnPropertyChanged(); }
        }

        private int _monatsSollzeit = 0;
        public int MonatsSollzeit
        {
            get => _monatsSollzeit;
            set { _monatsSollzeit = value; OnPropertyChanged(); }
        }

        private int _monatsSaldo = 0;
        public int MonatsSaldo
        {
            get => _monatsSaldo;
            set { _monatsSaldo = value; OnPropertyChanged(); }
        }

        public ICommand LadenCommand { get; }
        public ICommand SpeichernCommand { get; }
        public ICommand ExportierenCommand { get; }

        public ZeiterfassungsViewModel()
        {
            LadenCommand = new RelayCommand(Laden, ButtonsEnabled);
            SpeichernCommand = new RelayCommand(Speichern, ButtonsEnabled);
            ExportierenCommand = new RelayCommand(Exportieren, ButtonsEnabled);
            LadePersonen();
        }

        public void LadePersonen()
        {
            Personen.Clear();
            var personenListe = _service.getAllPersonen();
            foreach (var person in personenListe)
                Personen.Add(person);
        }

        private void Laden(object obj)
        {
            if (SelectedPerson == null || SelectedDate == default)
                return;
            var sollzeitZeiten = _service.GetSollzeitZeitenFürDatum(SelectedDate, SelectedPerson);
            Sollzeit = sollzeitZeiten == null ? 0 : sollzeitZeiten.FürDatum(SelectedDate);

            var saldo = _service.GetArbeitszeit(SelectedDate, SelectedPerson)?.Minuten ?? 0;
            Saldo = saldo - Sollzeit;
            Arbeitszeit = TimeSpan.FromMinutes(saldo).ToString(@"hh\:mm");

            MonatsSollzeit = _service.GetMonatsSollzeiten(SelectedDate, SelectedPerson);

            MonatsSaldo = _service.GetMonatsSaldo(SelectedDate, SelectedPerson);
        }

        private bool ButtonsEnabled(object obj)
        {
            return SelectedPerson != null && SelectedDate != default;
        }

        private void Speichern(object obj)
        {
            var minuten = 0;
            if (TimeSpan.TryParse(Arbeitszeit, out var ts))
                minuten = (int)ts.TotalMinutes;
            var arbeitszeit = new Arbeitszeit
            {
                Minuten = minuten,
                PersonenId = SelectedPerson.Id,
                Datum = SelectedDate
            };
            _service.SaveArbeitszeit(arbeitszeit);
        }

        private void Exportieren(object obj)
        {
            // Export-Logik hier implementieren
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}