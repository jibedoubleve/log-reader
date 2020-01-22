using CsvHelper;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Plugins.Csv.Config;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Probel.LogReader.Plugins.Csv.IO
{
    public class CsvFileReader
    {
        #region Fields

        private readonly string _path;
        private readonly QueryLogSettings _querylog;

        #endregion Fields

        #region Constructors

        public CsvFileReader(QueryLogSettings queryDay, string path)
        {
            _path = path;
            _querylog = queryDay;
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<LogRow> GetLogs()
        {
            var ci = CultureInfo.InvariantCulture;
            var encoding = Encoding.GetEncoding(_querylog.Encoding);

            using (var reader = new StreamReader(_path, encoding))
            using (var csv = new CsvReader(reader, ci))
            {
                csv.Configure(_querylog);

                var records = csv.GetRecords<LogRow>().ToList();
                return records;
            }
        }

        #endregion Methods
    }
}