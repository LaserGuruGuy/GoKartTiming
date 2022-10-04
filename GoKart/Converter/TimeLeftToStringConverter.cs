using System;
using System.Globalization;
using System.Windows.Data;

namespace GoKart.Converter
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeLeftToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? ((TimeSpan)value).Equals(TimeSpan.Zero) ? string.Empty : "Time: " + Format.TimeLeft((TimeSpan)value) : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}