using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Filters;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Probel.LogReader.Converters
{
    public class SubfilterToTextConverter : IMultiValueConverter
    {
        #region Methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                var msg = $"This value converter should have 2 elements. First should be of type '{typeof(FilterExpressionSettings)}' and second shoul dbe of type '{typeof(IFilterTranslator)}'";
                throw new NotSupportedException(msg);
            }

            if (values[0] is FilterExpressionSettings subfilter && values[1] is IFilterTranslator translator)
            {
                return translator.Translate(subfilter);
            }
            else { return string.Empty; }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}