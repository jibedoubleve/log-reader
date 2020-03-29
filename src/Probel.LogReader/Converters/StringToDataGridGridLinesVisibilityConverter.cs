using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Probel.LogReader.Converters
{
    public class StringToDataGridGridLinesVisibilityConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return Enum.TryParse(str, out DataGridGridLinesVisibility visibility) ? visibility : DataGridGridLinesVisibility.All;
            }
            else { return DataGridGridLinesVisibility.All; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataGridGridLinesVisibility visibility) { return visibility.ToString(); }
            else { return value.ToString(); }
        }

        #endregion Methods
    }
}