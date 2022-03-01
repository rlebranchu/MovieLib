using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MovieLib.Client.Converters
{
    class ObjectToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = true;

            if (value == null || value == DependencyProperty.UnsetValue)
            {
                result = false;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Nothing to do
            return null;
        }
    }
}
