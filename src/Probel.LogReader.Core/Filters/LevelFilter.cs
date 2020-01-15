using Probel.LogReader.Core.Configuration;
using System;
using System.Linq;

namespace Probel.LogReader.Core.Filters
{
    [Filter("Level", FilterOperations.List)]
    public class LevelFilter : BaseFilter
    {
        #region Constructors

        public LevelFilter() : base(FilterOperations.List)
        {
        }

        #endregion Constructors

        #region Methods

        protected override Func<LogRow, string, bool> GetFilter()
        {
            var levels = Array.ConvertAll(Operand.Split(','), p => p.Trim());
            switch (Operator.ToLower())
            {
                case "in": return ((r, t) => levels.Contains(r.Level));
                case "not in": return ((r, t) => !levels.Contains(r.Level));
                default: throw new NotSupportedException($"Cannot build a filter. Operator '{Operator}' is not supported.");
            }
        }

        #endregion Methods
    }
}