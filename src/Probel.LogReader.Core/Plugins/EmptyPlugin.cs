using Probel.LogReader.Core.Configuration;
using System;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Plugins
{
    public class EmptyPlugin : IPlugin
    {
        #region Methods

        public IEnumerable<DateTime> GetDays() => new List<DateTime>();

        public IEnumerable<LogRow> GetLogs(DateTime day, OrderBy orderby = OrderBy.Desc) => new List<LogRow>();

        #endregion Methods
    }
}