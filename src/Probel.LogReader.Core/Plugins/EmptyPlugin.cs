using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Constants;
using System;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Plugins
{
    public class EmptyPlugin : PluginBase
    {
        #region Methods

        public override IEnumerable<DateTime> GetDays(OrderBy orderby) => new List<DateTime>();

        public override IEnumerable<LogRow> GetLogs(DateTime day, OrderBy orderby = OrderBy.Desc) => new List<LogRow>();

        #endregion Methods
    }
}