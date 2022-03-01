using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MovieLib.Client.Converters
{
    class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Collapsed;

            if (value != DependencyProperty.UnsetValue && value is bool boolValue && boolValue)
            {
                result = Visibility.Visible;
            }

            if (parameter is string paramString && paramString == "!")
            {
                result = result == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
