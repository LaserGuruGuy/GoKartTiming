using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace GoKart
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class ComparisonToVisibleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (values[0] as string).Equals(values[1] as string) ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Visible;
            }
        }

        public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}