using Probel.LogReader.Core.Configuration;
using System;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Plugins
{
    public interface IPlugin
    {
        #region Methods

        IEnumerable<DateTime> GetDays();

        IEnumerable<LogRow> GetLogs(DateTime day, OrderBy orderby = OrderBy.Desc);

        #endregion Methods
    }
    public enum OrderBy
    {
        Asc,
        Desc,
        None,
    }
}