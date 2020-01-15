using System;
using System.Globalization;
using System.Windows.Data;

namespace Probel.LogReader.Converters
{
    public class StringToOneLinerConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                text = text.Replace(Environment.NewLine, " ");
                return text;
            }
            else { return value; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}