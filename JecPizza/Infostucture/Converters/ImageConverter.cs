using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace JecPizza.Infostucture.Converters
{
    public class ImageConverter : IValueConverter
    {

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? new BitmapImage(new Uri(value?.ToString() ?? "../Content/Images/1.jpg",UriKind.RelativeOrAbsolute)) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
