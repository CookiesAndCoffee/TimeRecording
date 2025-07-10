using System.Collections.ObjectModel;
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
                Load();
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
            SaveCommand = new RelayCommand(Save, () => !String.IsNullOrWhiteSpace(SelectedTargetTimeModel.Model));
            Load();
        }

        public void Add()
        {
            if (String.IsNullOrWhiteSpace(_newModel))
                return;
            var newModel = new TargetTimeModel { Model = _newModel };
            _service.SaveTargetTimeModel(newModel);
            Load();
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
            _service.SaveTargetTimeModel(SelectedTargetTimeModel);
            foreach (var times in TargetTimeModelTimes)
            {
                _service.SaveTargetTimeModelTimes(times);
            }
            Load();
        }

        public void Load()
        {
            TargetTimeModels.Clear();
            var models = _service.GetTargetTimeModels();
            foreach (var model in models)
                TargetTimeModels.Add(model);

            if (SelectedTargetTimeModel != null)
            {
                TargetTimeModelTimes.Clear();
                var times = _service.GetTargetTimeModelTimes()
                    .Where(t => t.TargetTimeModelId == SelectedTargetTimeModel.Id)
                    .ToList();
                foreach (var time in times)
                    TargetTimeModelTimes.Add(time);
            }
        }
    }
}
