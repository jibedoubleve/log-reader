using Newtonsoft.Json;
using System;

namespace Probel.LogReader.Core.Plugins
{
    public interface IPluginMetadata
    {
        #region Properties

        [JsonProperty("dll")]
        string Dll { get; }

        [JsonProperty("explanation")]
        string Description { get; }

        [JsonProperty("name")]
        string Name { get; }

        [JsonProperty("plugin-id")]
        Guid PluginId { get; }

        [JsonProperty("doc-url")]
        string DocUrl { get; }

        #endregion Properties
    }
}