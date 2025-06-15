using System.Windows;
using Zeiterfassung.views;

namespace Zeiterfassung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new ZeiterfassungsView());
        }
    }
}