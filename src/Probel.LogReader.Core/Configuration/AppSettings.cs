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
            Ui = new UiSettings();
        }

        #endregion Constructors

        #region Properties
        [JsonProperty("filters")]
        public IList<FilterSettings> Filters { get; }

        [JsonProperty("repositories")]
        public IList<RepositorySettings> Repositories { get; }

        [JsonProperty("ui")]
        public UiSettings Ui { get; set; }

        #endregion Properties
    }
}