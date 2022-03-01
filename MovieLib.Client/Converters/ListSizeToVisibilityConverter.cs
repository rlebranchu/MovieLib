using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MovieLib.Client.Converters
{
    class ListSizeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Collapsed;

            if (value != null || value != DependencyProperty.UnsetValue)
            {
                if(value is IList list && list.Count > 0)
                {
                    result = Visibility.Visible;
                }
            }

            if (parameter is string paramString && paramString == "!")
            {
                result = result == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
