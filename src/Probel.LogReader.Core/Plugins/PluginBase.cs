using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Constants;
using System;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        #region Properties

        protected RepositorySettings Settings { get; private set; }

        #endregion Properties

        #region Methods

        public abstract IEnumerable<DateTime> GetDays(OrderBy orderby = OrderBy.Desc);

        public abstract IEnumerable<LogRow> GetLogs(DateTime day, OrderBy orderby = OrderBy.Desc);

        public void Initialise(RepositorySettings settings) => Settings = settings;

        #endregion Methods
    }
}