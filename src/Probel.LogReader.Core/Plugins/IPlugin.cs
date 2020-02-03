using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Constants;
using System;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Plugins
{
    public interface IPlugin
    {
        #region Properties

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