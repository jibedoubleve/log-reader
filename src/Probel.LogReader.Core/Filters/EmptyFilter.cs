using Probel.LogReader.Core.Configuration;
using System;

namespace Probel.LogReader.Core.Filters
{
    public class EmptyFilter : BaseFilter
    {
        /// <remarks>
        /// This filter has a FilterOperations that does not mean anything as
        /// this is concidered as an empty filter.
        /// </remarks>
        public EmptyFilter() : base(FilterOperations.Comparition)
        {
        }
        #region Methods

        protected override Func<LogRow, string, bool> GetFilter() => (r, t) => true;

        #endregion Methods
    }
}