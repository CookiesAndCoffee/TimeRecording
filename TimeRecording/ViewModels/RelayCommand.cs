using System.Windows.Input;

namespace TimeRecording.ViewModels
{
    class RelayCommand : ICommand
    {
        private readonly Action<Object> _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action<Object> execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action execute, Func<bool> canExecute = null) : this((obj) => execute.Invoke(), canExecute)
        {
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
        public void Execute(object parameter) => _execute(parameter);
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
