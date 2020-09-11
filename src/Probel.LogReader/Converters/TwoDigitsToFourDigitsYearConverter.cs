using System;
using System.Globalization;
using System.Windows.Data;

namespace Probel.LogReader.Converters
{
    public class TwoDigitsToFourDigitsYearConverter : IValueConverter
    {
        #region Fields

        private string pattern = "dd MMMM yyyy";

        #endregion Fields

        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dt)
            {
                return dt.Year < 100 ? dt.AddYears(2000).ToString(pattern) : dt.ToString(pattern);
            }
            else { return value; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}