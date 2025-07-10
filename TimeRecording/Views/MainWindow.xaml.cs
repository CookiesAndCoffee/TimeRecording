using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using TimeRecording.ViewModels;
using TimeRecording.ViewModels.ViewData;
using TimeRecording.Views;

namespace TimeRecording
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowController _viewModel;

        public MainWindow(MainWindowController mainWindowViewModel)
        {
            InitializeComponent();
            _viewModel = mainWindowViewModel;
            DataContext = _viewModel;
        }

        private void NavListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.SelectedItem == null)
                return;

            UserControl view = _viewModel.SelectedItem.View switch
            {
                NavigatableViews.TimeRecordingView => App.ServiceProvider.GetService<TimeRecordingView>(),
                NavigatableViews.PersonView => App.ServiceProvider.GetService<PersonView>(),
                NavigatableViews.TargetTimeModelView => App.ServiceProvider.GetService<TargetTimeModelView>(),
                _ => throw new InvalidOperationException("View not found")
            };

            if (view != null)
                MainFrame.Navigate(view);
        }
    }
}