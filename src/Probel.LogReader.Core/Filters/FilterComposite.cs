using Probel.LogReader.Core.Configuration;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Filters
{
    public abstract class FilterComposite : IFilterComposite
    {
        #region Fields

        protected readonly IList<IFilterExpression> Filters = new List<IFilterExpression>();

        #endregion Fields

        #region Methods

        public void Add(params IFilterExpression[] filters)
        {
            foreach (var filter in filters) { Filters.Add(filter); }
        }

        public abstract IEnumerable<LogRow> Filter(IEnumerable<LogRow> rows);

        #endregion Methods
    }
}