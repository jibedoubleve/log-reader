using Probel.LogReader.Core.Configuration;
using System.Collections.Generic;
using System.Linq;

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
            var results = new List<IEnumerable<LogRow>>();
            foreach (var filter in Filters)
            {
                var r = filter.Filter(rows);
                results.Add(new List<LogRow>(r));
            }

            var intersection = results
                .Skip(1)
                .Aggregate(
                    new HashSet<LogRow>(results.First()),
                    (h, e) => { h.IntersectWith(e); return h; }
                );
            return intersection;            
        }

    }
}