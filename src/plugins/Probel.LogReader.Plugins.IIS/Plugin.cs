using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Constants;
using Probel.LogReader.Core.Helpers;
using Probel.LogReader.Core.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Probel.LogReader.Plugins.IIS
{
    public class Plugin : PluginBase
    {
        #region Fields

        private const string _pattern = @"^(?<datetime>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}) (?<s_ip>\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}) (?<cs_method>\w{0,9}) (?<cs_uri_stem>.*) (?<cs_uri_query>[^\s]*) (?<s_port>[^\s]*) (?<cs_username>[^\s]*) (?<c_ip>[^\s]*) (?<cs_user_agent>[^\s]*) (?<cs_referer>[^\s]*) (?<sc_status>[^\s]*) (?<sc_substatus>[^\s]*) (?<sc_win32_status>[^\s]*) (?<time_taken>[^\s]*)";
        private IEnumerable<LogSource> _dates;

        #endregion Fields

        #region Methods

        public override IEnumerable<DateTime> GetDays(OrderBy orderby = OrderBy.Desc)
        {
            var cs = (string.IsNullOrWhiteSpace(Settings.ConnectionString))
                ? @"u_ex(?<year>\d{2})(?<month>\d{2})(?<day>\d{2}).log"
                : Settings.ConnectionString;

            _dates = GetFiles(cs);

            switch (orderby)
            {
                case OrderBy.Asc:
                    return _dates.OrderBy(e => e.Day).Select(e => e.Day);

                case OrderBy.Desc:
                    return _dates.OrderByDescending(e => e.Day).Select(e => e.Day);

                case OrderBy.None:
                    return _dates.Select(e => e.Day);

                default: throw new NotSupportedException($"The orderby '{orderby}' is not supported!");
            }
        }

        public override IEnumerable<LogRow> GetLogs(DateTime day, OrderBy orderby = OrderBy.Desc)
        {
            var path = (from s in _dates
                        where s.Day.Date == day.Date
                        select s.FilePath).FirstOrDefault();
            try
            {
                var extractor = new LogExtractor(_pattern);

                return extractor.Extract(path);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Cannot get logs from file '{path}'", ex);
            }
        }

        private IEnumerable<LogSource> GetFiles(string dir)
        {
            dir = Environment.ExpandEnvironmentVariables(dir);

            var regex = new Regex(Settings.QueryDay);

            if (Directory.Exists(dir))
            {
                var files = (from f in Directory.GetFiles(dir)
                             where regex.IsMatch(f)
                             select new LogSource
                             {
                                 Day = regex.Match(f).AsDate(),
                                 FilePath = f
                             }).ToList();
                return files;
            }
            else { throw new FileNotFoundException($"Directory '{dir}' do not exist. Impossible to find CSV files."); }
        }

        #endregion Methods
    }
}