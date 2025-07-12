using System.Windows.Controls;
using TimeRecording.ViewModels;

namespace TimeRecording.Views
{
    /// <summary>
    /// Interaction logic for PersonView.xaml
    /// </summary>
    public partial class PersonView : UserControl
    {
        private PersonViewModel _viewModel;

        public PersonView(PersonViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;
        }

        private void PersonList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.LoadSelected();
        }

        private void PersonView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.LoadTargetTimeModels();
        }
    }
}
