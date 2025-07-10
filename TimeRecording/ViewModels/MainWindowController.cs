using System.Collections.ObjectModel;
using TimeRecording.ViewModels.ViewData;

namespace TimeRecording.ViewModels
{


    public class MainWindowController
    {
        public ObservableCollection<NavigationItem> NavigationItems { get; } = new()
        {
            new NavigationItem { Title = "Time Recording", SelectedIcon = "Clock", UnselectedIcon = "ClockOutline",  View = NavigatableViews.TimeRecordingView},
            new NavigationItem { Title = "Persons", SelectedIcon = "AccountMultiple", UnselectedIcon = "AccountMultipleOutline", View = NavigatableViews.PersonView },
            new NavigationItem { Title = "Models", SelectedIcon = "MapClock", UnselectedIcon = "MapClockOutline", View = NavigatableViews.TargetTimeModelView }
        };

        public NavigationItem SelectedItem { get; set; }
    }
}
