using System.Collections.ObjectModel;
using System.Windows.Input;
using TimeRecording.Models;
using TimeRecording.Services.Interfaces;

namespace TimeRecording.ViewModels
{
    public class PersonViewModel : AbstractViewModel<Person, IPersonService>
    {
        private string _personNo;
        public string PersonNo
        {
            get => _personNo;
            set
            {
                _personNo = value;
                OnPropertyChanged();
            }
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PersonTargetTimeModel> PersonTargetTimeModels { get; } = new();
        public ObservableCollection<TargetTimeModel> TargetTimeModels { get; } = new();

        public ICommand AddCommand { get; }
        public ICommand AddModelCommand { get; }

        private readonly ITargetTimeModelService _timeModelService;

        public PersonViewModel(IPersonService service, ITargetTimeModelService timeModelService) : base(service)
        {
            _timeModelService = timeModelService;
            AddCommand = new RelayCommand(AddPerson);
            AddModelCommand = new RelayCommand(AddModel, () => _selectedEntity != null);
        }

        private void AddPerson()
        {
            var person = new Person
            {
                PersonnelNumber = "",
                FirstName = "",
                LastName = ""
            };
            SelectedEntity = person;
        }

        public void LoadTargetTimeModels()
        {
            TargetTimeModels.Clear();
            foreach (var model in _timeModelService.GetAll())
                TargetTimeModels.Add(model);
        }

        private void AddModel()
        {
            PersonTargetTimeModels.Add(new PersonTargetTimeModel
            {
                Person = _selectedEntity,
                TargetTimeModel = TargetTimeModels.FirstOrDefault(new TargetTimeModel() { Model = "Create Model First" }),
                ValidFrom = DateTime.Now
            });
        }

        protected override bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(PersonNo) && !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName);
        }

        protected override void PreSave()
        {
            _selectedEntity.PersonnelNumber = PersonNo;
            _selectedEntity.LastName = LastName;
            _selectedEntity.FirstName = FirstName;
        }

        protected override void AfterSave()
        {
            _service.SavePersonTargetTimeModel(SelectedEntity, PersonTargetTimeModels.ToList());
        }

        public void LoadSelected()
        {
            if (_selectedEntity == null) return;
            PersonTargetTimeModels.Clear();
            PersonNo = _selectedEntity?.PersonnelNumber ?? string.Empty;
            FirstName = _selectedEntity?.FirstName ?? string.Empty;
            LastName = _selectedEntity?.LastName ?? string.Empty;
            var times = _service.GetPersonTargetTimeModels(_selectedEntity);
            foreach (var time in times)
                PersonTargetTimeModels.Add(time);
        }
    }
}
