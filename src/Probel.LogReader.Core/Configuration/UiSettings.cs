using Newtonsoft.Json;

namespace Probel.LogReader.Core.Configuration
{
    public class UiSettings
    {
        #region Properties

        [JsonProperty("show-logger")]
        public bool ShowLogger { get; set; }

        [JsonProperty("show-threadid")]
        public bool ShowThreadId { get; set; }

        #endregion Properties
    }
}