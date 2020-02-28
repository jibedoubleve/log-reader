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
        public bool isThreadIdVisible { get; set; }

        #endregion Properties
    }
}