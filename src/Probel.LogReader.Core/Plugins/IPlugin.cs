using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Constants;
using System;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Plugins
{
    public interface IDataListener
    {
        #region Events

        event EventHandler DataChanged;

        #endregion Events

        #region Methods

        /// <summary>
        /// Start listening on changes done on the repository.
        /// </summary>
        /// <param name="seconds">When value is "0" then will try to automatically notify on change as it occurs. If the plugin does not support this feature, it'll never update.
        /// Otherwise, indicates the seconds between two refreshes</param>
        /// <param name="day">Indicates on what day system should listen</param>
        void StartListening(DateTime day, int seconds = 0);

        /// <summary>
        /// Stop listening to changes done on the repository.
        /// </summary>
        void StopListening();

        #endregion Methods
    }

    public interface IPlugin : IDataListener
    {
        #region Properties

        /// <summary>
        /// Indicates whether this plugin can listen to changes
        /// </summary>
        bool CanListen { get; }

        string RepositoryName { get; }

        #endregion Properties

        #region Methods

        IEnumerable<DateTime> GetDays(OrderBy orderby = OrderBy.Desc);

        IEnumerable<LogRow> GetLogs(DateTime day, OrderBy orderby = OrderBy.Desc);

        void Initialise(RepositorySettings settings);

        /// <summary>
        /// Try to get the path of the log source if it is a file.
        /// If the connection string if not a path to a file, the
        /// method returns <c>False</c>, if it's a file it returns
        /// <c>True</c>
        /// </summary>
        /// <param name="path">If the connection string is a path, it'll be store
        /// into this variable</param>
        /// <returns><c>True</c> if the source if a file; otherwise <c>False</c></returns>
        bool TryGetFile(out string path);

        #endregion Methods
    }
}