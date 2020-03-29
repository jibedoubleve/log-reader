using Newtonsoft.Json;

namespace Probel.LogReader.Core.Configuration
{
    public class UiSettings
    {
        #region Properties

        [JsonProperty("log-sorted-ascending")]
        public bool IsLogOrderAsc { get; set; } = true;

        [JsonProperty("is-details-visible")]
        public bool IsDetailVisible { get; set; }

        [JsonProperty("is-logger-visible")]
        public bool IsLoggerVisible { get; set; }

        [JsonProperty("is-threadid-visible")]
        public bool IsThreadIdVisible { get; set; }

        /// <summary>
        /// Accepted values are
        ///  * All
        ///  * Horizontal
        ///  * None
        ///  * Vertical
        /// </summary>
        [JsonProperty("grid-line-visibility")]
        public string GridLineVisibility { get; set; } = "All";

        #endregion Properties
    }
}