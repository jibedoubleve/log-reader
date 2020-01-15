using Newtonsoft.Json;
using System;

namespace Probel.LogReader.Core.Configuration
{
    public static class RepositorySettingsExtensions
    {
        #region Methods

        public static bool HasValidId(this RepositorySettings repository)
        {
            if (repository == null) { return false; }

            return repository.PluginId != new Guid();
        }

        public static bool IsEmptyOrNull(this RepositorySettings repository)
        {
            if (repository == null) { return true; }

            var result = string.IsNullOrEmpty(repository.Name) == true
                     && repository.PluginId == new Guid();
            return result;
        }

        public static bool IsValid(this RepositorySettings repository) => !repository.IsEmptyOrNull();

        #endregion Methods
    }

    public class RepositorySettings
    {
        #region Properties

        [JsonProperty("connection-string")]
        public string ConnectionString { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("plugin-id")]
        public Guid PluginId { get; set; }

        [JsonProperty("query-day")]
        public string QueryDay { get; set; }

        [JsonProperty("query-log")]
        public string QueryLog { get; set; }

        #endregion Properties
    }
}