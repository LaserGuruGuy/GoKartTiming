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

            switch (improvedPosition)
            {
                case -1:
                    bitmap = new BitmapImage(new Uri("./Images/Up.png", UriKind.Relative));
                    break;
                case +1:
                    bitmap = new BitmapImage(new Uri("./Images/Down.png", UriKind.Relative));
                    break;
                default:
                    break;
            }

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
