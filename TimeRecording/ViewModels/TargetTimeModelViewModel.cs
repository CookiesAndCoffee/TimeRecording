using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TimeRecording.Models;
using TimeRecording.Services;

namespace TimeRecording.ViewModels
{
    public class TargetTimeModelViewModel : AbstractViewModel
    {
        private TargetTimeModel _targetTimeModel;
        public TargetTimeModel SelectedTargetTimeModel
        {
            get => _targetTimeModel;
            set
            {
                _targetTimeModel = value;
                OnPropertyChanged();
                HasSelectedTargetTimeModel = _targetTimeModel != null ? Visibility.Visible : Visibility.Hidden;
                HasNotSelectedTargetTimeModel = _targetTimeModel == null ? Visibility.Visible : Visibility.Hidden;
            }
        }

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

        public ObservableCollection<TargetTimeModel> TargetTimeModels { get; } = new();
        public ObservableCollection<TargetTimeModelTimes> TargetTimeModelTimes { get; } = new();
        public ICommand AddCommand { get; }
        public ICommand AddTimesCommand { get; }
        public ICommand SaveCommand { get; }

        private TimeRecordingService _service;

        public TargetTimeModelViewModel(TimeRecordingService service)
        {
            _service = service;
            AddCommand = new RelayCommand(Add, () => !String.IsNullOrWhiteSpace(_newModel));
            AddTimesCommand = new RelayCommand(AddTimes, () => SelectedTargetTimeModel != null);
            SaveCommand = new RelayCommand(Save, () => !String.IsNullOrWhiteSpace(_changeModel));
            LoadList();
        }

        public void Add()
        {
            if (String.IsNullOrWhiteSpace(_newModel))
                return;
            var newModel = new TargetTimeModel { Model = _newModel };
            _service.SaveTargetTimeModel(newModel);
            LoadList();
            NewModel = string.Empty;
        }

        public void AddTimes()
        {
            TargetTimeModelTimes.Add(new TargetTimeModelTimes
            {
                TargetTimeModel = SelectedTargetTimeModel,
                TargetTimeModelId = SelectedTargetTimeModel.Id,
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

        public void Save()
        {
            if (SelectedTargetTimeModel == null)
                return;
            _targetTimeModel.Model = _changeModel;
            _service.SaveTargetTimeModel(_targetTimeModel);
            foreach (var times in TargetTimeModelTimes)
            {
                _service.SaveTargetTimeModelTimes(times);
            }
            LoadSelected();
        }

        public void LoadList()
        {
            TargetTimeModels.Clear();
            var models = _service.GetTargetTimeModels();
            foreach (var model in models)
                TargetTimeModels.Add(model);
        }

        public void LoadSelected()
        {
            TargetTimeModelTimes.Clear();
            ChangeModel = SelectedTargetTimeModel?.Model ?? string.Empty;
            var times = _service.GetTargetTimeModelTimes()
                .Where(t => t.TargetTimeModelId == SelectedTargetTimeModel?.Id)
                .ToList();
            foreach (var time in times)
                TargetTimeModelTimes.Add(time);
        }
    }
}
