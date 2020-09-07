using Newtonsoft.Json;
using System;

namespace Probel.LogReader.Core.Configuration
{
    /// <summary>
    /// It is a bind of a filter with a repository. This means the specified filter
    /// can be executed in the specified repository
    /// </summary>
    public class RepositoryFilterSettings
    {
        #region Properties

        [JsonProperty("id-filter")]
        public Guid FilterId { get; set; }

        [JsonProperty("id-repository")]
        public Guid RepositoryId { get; set; }

        #endregion Properties
    }
}