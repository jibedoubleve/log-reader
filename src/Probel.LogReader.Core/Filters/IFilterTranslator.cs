using Probel.LogReader.Core.Configuration;

namespace Probel.LogReader.Core.Filters
{
    public interface IFilterTranslator
    {
        #region Methods

        string Translate(FilterSettings stg);

        string Translate(FilterExpressionSettings exp);

        #endregion Methods
    }
}