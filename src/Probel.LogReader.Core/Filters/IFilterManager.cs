using Probel.LogReader.Core.Configuration;
using System;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Filters
{
    public interface IFilterManager
    {
        #region Methods

        IFilterComposite Build(IEnumerable<FilterExpressionSettings> expression, string @operator);

        IFilterComposite Build(Guid id);

        #endregion Methods
    }
}