﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace GoKart.Converter
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class LapsLeftToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? value.Equals(0) ? string.Empty : ((int)value + (int)value > 1 ? " laps" : " lap") : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}