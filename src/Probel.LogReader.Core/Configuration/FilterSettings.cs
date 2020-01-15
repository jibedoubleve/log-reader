using Newtonsoft.Json;
using Probel.LogReader.Core.Constants;
using Probel.LogReader.Core.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Probel.LogReader.Core.Configuration
{
    public static class FilterSettingsExtension
    {
        #region Methods

        public static bool HasDefaultFilter(this AppSettings settings)
        {
            var cnt = (from f in settings.Filters
                       where f.Id == FilterSettings.NoFilter.Id
                       select f).Count() > 0;
            return cnt;
        }

        #endregion Methods
    }

    public class FilterSettings
    {
        #region Constructors

        public FilterSettings(string name) : this()
        {
            Name = name;
        }

        public FilterSettings()
        {
            Expression = new List<FilterExpressionSettings>();
            Id = Guid.NewGuid();
        }

        #endregion Constructors

        #region Properties

        public static FilterSettings NoFilter
        {
            get
            {
                var expression = new List<FilterExpressionSettings>()
                {
                    new FilterExpressionSettings()
                    {
                        Operation = FilterType.Level,
                        Operand = "",
                        Operator = "not in"
                    }
                };

                var res = new FilterSettings()
                {
                    Name = Resource.Filter_AllLogs,
                    Id = new Guid("01f36d50-5051-4def-a4b4-3665c0b3b00d"), //Always the same id
                    Expression = expression,
                };
                return res;
            }
        }

        [JsonProperty("expression")]
        public IList<FilterExpressionSettings> Expression { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion Properties
    }
}