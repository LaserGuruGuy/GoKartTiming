using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GoKart.Converter
{
    [ValueConversion(typeof(int), typeof(BitmapImage))]
    public class PositionToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var position = value as int?;

            BitmapImage bitmap = null;

            switch (position)
            {
                case 1:
                    bitmap = new BitmapImage(new Uri("./Images/Gold.png", UriKind.Relative));
                    break;
                case 2:
                    bitmap = new BitmapImage(new Uri("./Images/Silver.png", UriKind.Relative));
                    break;
                case 3:
                    bitmap = new BitmapImage(new Uri("./Images/Bronze.png", UriKind.Relative));
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
