using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Probel.LogReader.Converters
{
    public class InvertBooleanToVisibilityConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b) { return !b; }
            else { return value; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}