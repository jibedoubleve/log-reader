using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Constants;
using Probel.LogReader.Core.Helpers;
using System;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        #region Properties

        public string RepositoryName => Settings?.Name ?? "EMPTY";
        protected RepositorySettings Settings { get; private set; }

        #endregion Properties

        #region Methods

        public abstract IEnumerable<DateTime> GetDays(OrderBy orderby = OrderBy.Desc);

        public abstract IEnumerable<LogRow> GetLogs(DateTime day, OrderBy orderby = OrderBy.Desc);

        public void Initialise(RepositorySettings settings) => Settings = settings;

        public bool IsFile()
        {
            var path = Environment.ExpandEnvironmentVariables(Settings.ConnectionString);
            return path.IsValidPath();
        }

        public bool TryGetFile(out string path)
        {
            path = null;
            if (string.IsNullOrEmpty(Settings.ConnectionString)) { return false; }

            var p = Environment.ExpandEnvironmentVariables(Settings.ConnectionString);

            if (p.IsValidPath())
            {
                path = p;
                return true;
            }
            else { return false; }
        }

        #endregion Methods
    }
}