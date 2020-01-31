using Probel.LogReader.Core.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Probel.LogReader.Core.Filters
{
    public class OrFilterComposite : FilterComposite
    {
        #region Methods

        /// <summary>
        /// This filter works as a OR.
        /// </summary>
        /// <param name="rows">Input enumeration of <see cref="LogRow"/> to be filterd</param>
        /// <returns>Filters enumeration of <see cref="LogRow"/></returns>
        public override IEnumerable<LogRow> Filter(IEnumerable<LogRow> rows)
        {
            var result = new List<LogRow>();
            foreach (var filter in Filters) { result.AddRange(filter.Filter(rows)); }

            return result.Distinct(new LogRowEqualityComparer());
        }

        #endregion Methods
    }
}