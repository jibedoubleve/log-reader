using Probel.LogReader.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Probel.LogReader.Core.Filters
{
    [DebuggerDisplay("{Operator} {Operand} | Type: {Type}")]
    public abstract class BaseFilter : IFilterExpression
    {
        #region Constructors

        public BaseFilter(FilterOperations operation)
        {
            Operation = operation;
        }

        #endregion Constructors

        #region Properties

        public string Operand { get; set; }

        public FilterOperations Operation { get; }
        public string Operator { get; set; }

        public string Type { get; set; }

        #endregion Properties

        #region Methods

        public IEnumerable<LogRow> Filter(IEnumerable<LogRow> rows)
        {
            if (rows == null) { return new List<LogRow>(); }

            var res = from r in rows
                      where DoFilter(r)
                      select r;
            return res;
        }

        protected abstract Func<LogRow, string, bool> GetFilter();

        private bool DoFilter(LogRow r)
        {
            var filter = GetFilter();
            return filter(r, Operand);
        }

        #endregion Methods
    }
}