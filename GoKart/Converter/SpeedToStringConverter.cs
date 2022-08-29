using System;
using System.Globalization;
using System.Windows.Data;

namespace GoKart
{
    [ValueConversion(typeof(float), typeof(string))]
    public sealed class SpeedToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? Format.Speed((float)value) : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}