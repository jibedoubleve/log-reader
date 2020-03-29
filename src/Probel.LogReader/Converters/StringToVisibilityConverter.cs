using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Probel.LogReader.Converters
{
    public class StringToVisibilityConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) { return Visibility.Collapsed; }
            else if (value is string str)
            {
                return string.IsNullOrWhiteSpace(str) ? Visibility.Collapsed : (object)Visibility.Visible;
            }
            else { return Visibility.Visible; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}