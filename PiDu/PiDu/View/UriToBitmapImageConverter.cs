using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace PiDu.View
{
    public class UriToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return new BitmapImage();

            BitmapImage image = new BitmapImage();
            try
            {
                image.BeginInit();
                image.UriSource = new Uri(value as string);
                image.EndInit();
            }
            catch (Exception ex)
            {
                return new BitmapImage();
                //do nothing
            }

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
