using Probel.LogReader.Core.Configuration;
using System;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Plugins
{
    public interface IPlugin
    {
        #region Methods
        void Initialise(RepositorySettings settings);

        IEnumerable<DateTime> GetDays(OrderBy orderby = OrderBy.Desc);

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