using Probel.LogReader.Core.Configuration;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Filters
{
    public interface IFilter
    {
        #region Methods

        IEnumerable<LogRow> Filter(IEnumerable<LogRow> rows);

        #endregion Methods
    }
}