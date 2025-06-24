using System.Globalization;
using System.Windows.Data;

namespace TimeRecording.Views.Converter
{
    public class TimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && TimeSpan.TryParse(s, out var ts))
                return ts.ToString(@"hh\:mm");
            if (value is int i)
                return TimeSpan.FromMinutes(i).ToString(@"hh\:mm");
            return "00:00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && TimeSpan.TryParse(s, out var ts))
                return ts.ToString(@"hh\:mm");
            return "00:00";
        }
    }
}