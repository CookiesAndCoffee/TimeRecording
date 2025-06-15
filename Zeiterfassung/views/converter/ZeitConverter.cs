using System.Globalization;
using System.Windows.Data;

namespace Zeiterfassung.views.converter
{
    internal class ZeitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan ts;
            if (value is string s && TimeSpan.TryParse(s, out ts))
                return FormatTimeSpan(ts);
            if (value is int i)
                return FormatTimeSpan(TimeSpan.FromMinutes(i));
            return "00:00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && TimeSpan.TryParse(s, out var ts))
                return ts.ToString(@"hh\:mm");
            return "00:00";
        }

        private static string FormatTimeSpan(TimeSpan ts)
        {
            int totalMinutes = (int)ts.TotalMinutes;
            int hours = totalMinutes / 60;
            int minutes = Math.Abs(totalMinutes % 60);
            return $"{hours}:{minutes:D2}";
        }
    }
}
