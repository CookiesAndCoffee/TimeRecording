using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TimeRecording.Services.Interfaces;

namespace TimeRecording.ViewModels
{
    public abstract class AbstractViewModel<E, I> : INotifyPropertyChanged where E : class where I : IService<E>
    {
        protected E _selectedEntity;
        public E SelectedEntity
        {
            get => _selectedEntity;
            set
            {
                _selectedEntity = value;
                OnPropertyChanged();
                HasSelectedEntity = _selectedEntity != null;
            }
        }

        private bool _hasSelectedEntity = false;
        public bool HasSelectedEntity
        {
            get => _hasSelectedEntity;
            set
            {
                _hasSelectedEntity = value;
                OnPropertyChanged(nameof(HasSelectedEntity));
            }
        }

        protected readonly I _service;

        public ObservableCollection<E> EntityList { get; } = new();
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public AbstractViewModel(I service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            SaveCommand = new RelayCommand(() => Save(), CanSave);
            DeleteCommand = new RelayCommand(() => Delete(), () => _selectedEntity != null);
            Load();
        }

        abstract protected bool CanSave();

        protected void Load()
        {
            EntityList.Clear();
            var items = _service.GetAll();
            foreach (var item in items)
                EntityList.Add(item);
        }

        protected void Save()
        {
            if (_selectedEntity == null)
                return;
            PreSave();
            _service.Save(_selectedEntity);
            AfterSave();
            Load();
        }

        protected virtual void PreSave()
        {
        }

        protected virtual void AfterSave()
        {
        }

        protected void Delete()
        {
            if (_selectedEntity == null)
                return;
            _service.Delete(_selectedEntity);
            SelectedEntity = null;
            Load();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
