using System.Windows.Controls;
using TimeRecording.ViewModels;

namespace TimeRecording.Views
{
    public partial class TimeRecordingView : UserControl
    {
        public TimeRecordingView(TimeRecordingViewModel timeRecordingViewModel)
        {
            InitializeComponent();
            var viewModel = timeRecordingViewModel;
            viewModel.View = this;
            DataContext = viewModel;
        }
    }
}
