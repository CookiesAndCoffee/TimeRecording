using System.Windows.Controls;
using Zeiterfassung.viewmodels;

namespace Zeiterfassung.views
{
    public partial class ZeiterfassungsView : UserControl
    {
        public ZeiterfassungsView()
        {
            InitializeComponent();
            var viewModel = new ZeiterfassungsViewModel
            {
                View = this
            };
            DataContext = viewModel;
        }
    }
}
