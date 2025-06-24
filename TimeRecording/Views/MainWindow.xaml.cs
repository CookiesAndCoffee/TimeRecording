using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TimeRecording.Views;

namespace TimeRecording
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var timeRecordingView = App.ServiceProvider.GetService<TimeRecordingView>();
            MainFrame.Navigate(timeRecordingView);
        }
    }
}