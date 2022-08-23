using System;
using System.Globalization;
using System.Windows.Data;

namespace GoKart
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public sealed class TimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? Format.LapTime((TimeSpan)value) : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
