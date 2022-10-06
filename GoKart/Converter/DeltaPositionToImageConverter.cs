using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GoKart.Converter
{
    [ValueConversion(typeof(int), typeof(BitmapImage))]
    public class DeltaPositionToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var deltaPosition = value as int?;

            BitmapImage bitmap = null;

            if (deltaPosition < 0)
            {
                bitmap = new BitmapImage(new Uri("./Images/Up.png", UriKind.Relative));
            }
            else if (deltaPosition > 0)
            {
                bitmap = new BitmapImage(new Uri("./Images/Down.png", UriKind.Relative));
            }

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
