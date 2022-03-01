using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using TMDbLib.Objects.Search;

namespace MovieLib.Client.Converters
{
    class ListContainsToFilePathConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            String STAREMPTYFILE = "\\Resources\\Images\\star_empty.png";
            String STARFILE = "\\Resources\\Images\\star.png";
            String path = STAREMPTYFILE;

            if (values[0] != null || values[0] != DependencyProperty.UnsetValue)
            {
                if (values[1] is IList list && list.Cast<SearchMovie>().ToArray().ToList().Any(item => item.Id == ((SearchMovie) values[0]).Id))
                {
                    path = STARFILE;
                }
            }

            if (parameter is string paramString && paramString == "!")
            {
                path = path == STAREMPTYFILE ? STARFILE : STAREMPTYFILE;
            }
            
            return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
