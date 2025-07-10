using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TimeRecording.ViewModels
{
    public abstract class AbstractViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
