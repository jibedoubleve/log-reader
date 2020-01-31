using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Constants;
using System;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Plugins
{
    public interface IPlugin
    {
        #region Methods

        IEnumerable<DateTime> GetDays(OrderBy orderby = OrderBy.Desc);

        IEnumerable<LogRow> GetLogs(DateTime day, OrderBy orderby = OrderBy.Desc);

        void Initialise(RepositorySettings settings);

        #endregion Methods
    }
}