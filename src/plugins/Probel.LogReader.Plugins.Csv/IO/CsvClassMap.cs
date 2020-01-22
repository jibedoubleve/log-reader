using CsvHelper.Configuration;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Plugins.Csv.Config;

namespace Probel.LogReader.Plugins.Csv.IO
{
    internal class CsvClassMap : ClassMap<LogRow>
    {
        #region Constructors

        public CsvClassMap(QueryLogSettings queryday)
        {
            Map(m => m.Exception).Name(queryday.Exception).Optional();
            Map(m => m.Level).Name(queryday.Level);
            Map(m => m.Logger).Name(queryday.Logger);
            Map(m => m.Message).Name(queryday.Message);
            Map(m => m.ThreadId).Name(queryday.ThreadId);
            Map(m => m.Time).Name(queryday.Time);
        }

        #endregion Constructors
    }
}