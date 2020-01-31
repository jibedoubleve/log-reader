using Newtonsoft.Json;

namespace Probel.LogReader.Core.Configuration
{
    public class FilterExpressionSettings
    {
        #region Properties

        [JsonProperty("operand")]
        public string Operand { get; set; }

        [JsonProperty("type")]
        public string Operation { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; }

        #endregion Properties
    }
}