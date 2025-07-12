using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TimeRecording.Models;
using TimeRecording.Services.Interfaces;

namespace TimeRecording.ViewModels
{
    public class TargetTimeModelViewModel : AbstractViewModel<TargetTimeModel, ITargetTimeModelService>
    {
        private string _newModel;
        public string NewModel
        {
            get => _newModel;
            set
            {
                _newModel = value;
                OnPropertyChanged();
            }
        }

        private string _changeModel;
        public string ChangeModel
        {
            get => _changeModel;
            set
            {
                _changeModel = value;
                OnPropertyChanged();
            }
        }

        private Visibility _hasSelectedTargetTimeModel = Visibility.Hidden;
        public Visibility HasSelectedTargetTimeModel
        {
            get => _hasSelectedTargetTimeModel;
            set
            {
                _hasSelectedTargetTimeModel = value;
                OnPropertyChanged();
            }
        }

        private Visibility _hasNotSelectedTargetTimeModel = Visibility.Visible;
        public Visibility HasNotSelectedTargetTimeModel
        {
            get => _hasNotSelectedTargetTimeModel;
            set
            {
                _hasNotSelectedTargetTimeModel = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TargetTimeModelTimes> TargetTimeModelTimes { get; } = new();
        public ICommand AddCommand { get; }
        public ICommand AddTimesCommand { get; }

        public TargetTimeModelViewModel(ITargetTimeModelService service) : base(service)
        {
            AddCommand = new RelayCommand(Add, () => !String.IsNullOrWhiteSpace(_newModel));
            AddTimesCommand = new RelayCommand(AddTimes, () => SelectedEntity != null);
        }

        protected override bool CanSave()
        {
            return !String.IsNullOrWhiteSpace(_changeModel);
        }

        public void Add()
        {
            if (String.IsNullOrWhiteSpace(_newModel))
                return;
            var newModel = new TargetTimeModel { Model = _newModel };
            _service.Save(newModel);
            Load();
            NewModel = string.Empty;
        }

        public void AddTimes()
        {
            TargetTimeModelTimes.Add(new TargetTimeModelTimes
            {
                TargetTimeModel = SelectedEntity,
                ValidFrom = DateTime.Now,
                Monday = 0,
                Tuesday = 0,
                Wednesday = 0,
                Thursday = 0,
                Friday = 0,
                Saturday = 0,
                Sunday = 0
            });
        }

        protected override void PreSave()
        {
            _selectedEntity.Model = _changeModel;
        }

        protected override void AfterSave()
        {
            _service.SaveTargetTimeModelTimes(SelectedEntity, TargetTimeModelTimes.ToList());
        }

        public void LoadSelected()
        {
            if (_selectedEntity == null) return;
            TargetTimeModelTimes.Clear();
            ChangeModel = _selectedEntity?.Model ?? string.Empty;
            var times = _service.GetTargetTimeModelTimes(_selectedEntity);
            foreach (var time in times)
                TargetTimeModelTimes.Add(time);
        }
    }
}
