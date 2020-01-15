using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Probel.LogReader.Converters
{
    public class TextToVisibilityConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                return (string.IsNullOrEmpty(text)) ? Visibility.Collapsed : Visibility.Visible;
            }
            else { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}