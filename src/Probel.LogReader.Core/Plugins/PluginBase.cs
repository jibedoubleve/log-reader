using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Constants;
using Probel.LogReader.Core.Helpers;
using System;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        #region Events

        public event EventHandler DataChanged;

        #endregion Events

        #region Properties

        public virtual bool CanListen => false;

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

        public virtual void StartListening(DateTime day, int seconds = 0)
        {
            /* By default act as listening is not supported but throws
             * an exception if CanListen is set to true and this method
             * is not overriden
             */
            if (CanListen)
            {
                throw new NotImplementedException($"The method '{(nameof(StartListening))}' should be implemented if you activate '{(nameof(CanListen))}'");
            }
        }

        public virtual void StopListening()
        {
            /* By default act as listening is not supported but throws
             * an exception if CanListen is set to true and this method
             * is not overriden
             */
            if (CanListen)
            {
                throw new NotImplementedException($"The method '{(nameof(StopListening))}' should be implemented if you activate '{(nameof(CanListen))}'");
            }
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

        protected void OnChanged() => DataChanged?.Invoke(this, EventArgs.Empty);

        #endregion Methods
    }
}