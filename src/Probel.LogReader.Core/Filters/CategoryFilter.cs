using Probel.LogReader.Core.Configuration;
using System;
using System.Linq;

namespace Probel.LogReader.Core.Filters
{
    [Filter("Category", FilterOperations.List)]
    public class CategoryFilter : BaseFilter
    {
        #region Constructors

        public CategoryFilter() : base(FilterOperations.List)
        {
        }

        #endregion Constructors

        #region Methods

        protected override Func<LogRow, string, bool> GetFilter()
        {
            var categories = Array.ConvertAll(Operand.Split(','), p => p.Trim());
            switch (Operator.ToLower())
            {
                case "in": return ((r, t) => categories.Contains(r.Logger.ToLower()));
                case "not in": return ((r, t) => !categories.Contains(r.Logger));
                default: throw new NotSupportedException($"Cannot build a filter. Operator '{Operator}' is not supported.");
            }
        }

        #endregion Methods
    }
}