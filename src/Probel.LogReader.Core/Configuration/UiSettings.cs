using Newtonsoft.Json;

namespace Probel.LogReader.Core.Configuration
{
    public class UiSettings
    {
        #region Properties

        /// <summary>
        /// Accepted values are
        ///  * All
        ///  * Horizontal
        ///  * None
        ///  * Vertical
        /// </summary>
        [JsonProperty("grid-line-visibility")]
        public string GridLineVisibility { get; set; } = "All";

        [JsonProperty("is-logger-visible")]
        public bool IsLoggerVisible { get; set; }

        [JsonProperty("log-sorted-ascending")]
        public bool IsLogOrderAsc { get; set; } = true;

        [JsonProperty("is-threadid-visible")]
        public bool IsThreadIdVisible { get; set; }

        [JsonProperty("layout-file")]
        public string LayoutFile { get; set; } = "%appdata%\\probel\\log-reader\\layout.bin";

        [JsonProperty("show-layout-buttons")]
        public bool ShowLayoutButtons { get; set; } = false;

        #endregion Properties
    }
}