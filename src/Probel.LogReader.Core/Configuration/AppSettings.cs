using Newtonsoft.Json;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Configuration
{
    public class AppSettings
    {
        #region Constructors

        public AppSettings()
        {
            Filters = new List<FilterSettings>();
            Repositories = new List<RepositorySettings>();
            RepositoryFilters = new List<RepositoryFilterSettings>();
            Ui = new UiSettings();
        }

        #endregion Constructors

        #region Properties
        [JsonProperty("filters")]
        public IList<FilterSettings> Filters { get; }

        [JsonProperty("repositories")]
        public IList<RepositorySettings> Repositories { get; }

        [JsonProperty("repository-filters")]
        public IList<RepositoryFilterSettings> RepositoryFilters { get; set; }

        [JsonProperty("ui")]
        public UiSettings Ui { get; set; }

        #endregion Properties
    }
}