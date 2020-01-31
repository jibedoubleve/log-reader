using System;
using System.Globalization;
using System.Windows.Data;

namespace Probel.LogReader.Converters
{
    public class UppercaseConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str) { return str.ToUpper(); }
            else { return value; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}