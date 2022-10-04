using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GoKart.Converter
{
    [ValueConversion(typeof(int), typeof(BitmapImage))]
    public class ImprovedPositionToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var improvedPosition = value as int?;

            BitmapImage bitmap = null;

            if (improvedPosition < 0)
            {
                bitmap = new BitmapImage(new Uri("./Images/Up.png", UriKind.Relative));
            }
            else if (improvedPosition > 0)
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
