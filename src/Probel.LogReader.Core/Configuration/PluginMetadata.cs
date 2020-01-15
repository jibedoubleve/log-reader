using Newtonsoft.Json;
using System;

namespace Probel.LogReader.Core.Plugins
{
    public class PluginMetadata : IPluginMetadata
    {
        #region Properties

        [JsonProperty("dll")]
        public string Dll { get; set; }

        [JsonProperty("doc-url")]
        public string DocUrl { get; set; }

        [JsonProperty("explanation")]
        public string Explanation { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Should be the guid of the plugin. Information is in the
        /// plugin.config.json
        /// </summary>
        [JsonProperty("plugin-id")]
        public Guid PluginId { get; set; }

        #endregion Properties
    }
}