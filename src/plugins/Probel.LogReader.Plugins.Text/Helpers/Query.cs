using System.Linq;

namespace Probel.LogReader.Plugins.Text.Helpers
{
    internal partial class FileFinder
    {
        #region Classes

        private class Query
        {
            #region Fields

            public const string CreationDate = "<creation date>";
            public const string LastUpdate = "<last update>";
            private static string[] _supportedQueries = new string[] { LastUpdate, CreationDate };
            private readonly string _queryDay;

            #endregion Fields

            #region Constructors

            public Query(string queryDay)
            {
                _queryDay = queryDay.ToLower();
            }

            #endregion Constructors

            #region Properties

            public object Value => _queryDay ?? "N.A.";

            internal bool IsOnCreatioDate => _queryDay == CreationDate;

            internal bool IsOnLastWrite => _queryDay == LastUpdate;

            #endregion Properties

            #region Methods

            public static bool Contains(string query) => _supportedQueries.Contains(query);

            #endregion Methods
        }

        #endregion Classes
    }
}