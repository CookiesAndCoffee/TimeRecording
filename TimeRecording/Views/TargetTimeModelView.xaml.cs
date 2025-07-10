using System.Windows.Controls;
using TimeRecording.ViewModels;

namespace TimeRecording.Views
{
    /// <summary>
    /// Interaction logic for TargetTimeModelView.xaml
    /// </summary>
    public partial class TargetTimeModelView : UserControl
    {
        private TargetTimeModelViewModel _viewModel;

        public TargetTimeModelView(TargetTimeModelViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }
    }
}
