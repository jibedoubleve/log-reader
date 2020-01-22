using CsvHelper;
using Probel.LogReader.Plugins.Csv.Config;

namespace Probel.LogReader.Plugins.Csv.IO
{
    public static class CsvReaderHelper
    {
        #region Methods

        public static void Configure(this CsvReader csv, QueryLogSettings stg)
        {
            var map = new CsvClassMap(stg);
            csv.Configuration.RegisterClassMap(map);
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.MissingFieldFound = null;
            csv.Configuration.HeaderValidated = null;
            csv.Configuration.Delimiter = stg.Delimiter;
            csv.Configuration.PrepareHeaderForMatch = (header, idx) => header.ToLower();
        }

        #endregion Methods
    }
}