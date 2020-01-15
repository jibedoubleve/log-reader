using Probel.LogReader.Core.Configuration;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Filters
{
    public class AndFilterComposite : FilterComposite
    {

        /// <summary>
        /// This filter works as a AND.
        /// </summary>
        /// <param name="rows">Input enumeration of <see cref="LogRow"/> to be filterd</param>
        /// <returns>Filters enumeration of <see cref="LogRow"/></returns>
        public override IEnumerable<LogRow> Filter(IEnumerable<LogRow> rows)
        {
            foreach (var filter in Filters) { rows = filter.Filter(rows); }
            return rows;
        }

    }
}