using System;
using System.Globalization;
using System.Windows.Data;

namespace GoKart
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class ActualHeatStartToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? "Actual Heat Start: " + (int)value : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}