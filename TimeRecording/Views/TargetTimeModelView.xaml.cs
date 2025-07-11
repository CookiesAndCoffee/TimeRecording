using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            initGridView();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void TargetTimeModels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.LoadSelected();
        }

        private void initGridView()
        {
            gridView.Columns.Add(CreateDayColumn("Monday"));
            gridView.Columns.Add(CreateDayColumn("Tuesday"));
            gridView.Columns.Add(CreateDayColumn("Wednesday"));
            gridView.Columns.Add(CreateDayColumn("Thursday"));
            gridView.Columns.Add(CreateDayColumn("Friday"));
            gridView.Columns.Add(CreateDayColumn("Saturday"));
            gridView.Columns.Add(CreateDayColumn("Sunday"));
        }

        private GridViewColumn CreateDayColumn(string header)
        {
            var binding = new Binding(header)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            var factory = new FrameworkElementFactory(typeof(TextBox));
            factory.SetBinding(TextBox.TextProperty, binding);
            factory.SetValue(TextBox.WidthProperty, 40.0);
            factory.SetValue(TextBox.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            factory.SetValue(TextBox.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(TextBox.MarginProperty, new Thickness(5, 0, 0, 0));

            var template = new DataTemplate { VisualTree = factory };

            return new GridViewColumn
            {
                Header = header,
                CellTemplate = template
            };
        }

    }
}
