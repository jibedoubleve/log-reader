using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Constants;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Plugins.Csv.Config;
using Probel.LogReader.Plugins.Csv.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Probel.LogReader.Plugins.Csv
{
    public class CsvPlugin : PluginBase
    {
        #region Fields

        private FileSystemWatcher _fw;

        #endregion Fields

        #region Properties

        public override bool CanListen => true;

        #endregion Properties

        #region Methods

        public override IEnumerable<DateTime> GetDays(OrderBy orderby = OrderBy.Desc)
        {
            var dates = GetDays();

            switch (orderby)
            {
                case OrderBy.Asc: return dates.Keys.OrderBy(e => e);
                case OrderBy.Desc: return dates.Keys.OrderByDescending(e => e);
                case OrderBy.None: return dates.Keys;
                default: throw new NotSupportedException($"This sort '{orderby}' is not supported!");
            }
        }

        public override IEnumerable<LogRow> GetLogs(DateTime day, OrderBy orderby = OrderBy.Desc)
        {
            var s = GetQueryLogSettings();
            var d = GetDays();

            if (d.ContainsKey(day.Date))
            {
                var reader = new CsvFileReader(s, d[day.Date]);
                return reader.GetLogs();
            }
            return new List<LogRow>();
        }

        public override void StartListening(DateTime day, int seconds = 0)
        {
            var days = GetDays();
            if (days.ContainsKey(day))
            {
                var path = days[day];
                InitialiseFileWatcher(path);
            }
            else { throw new NotSupportedException($"No logs found to the specifie day '{day}'"); }
        }

        public override void StopListening() => ClearFileWatcher();

        private void ClearFileWatcher()
        {
            if (_fw != null)
            {
                _fw.Changed -= OnFileChanged;
                _fw = null;
            }
        }

        private Dictionary<DateTime, string> GetDays()
        {
            var s = GetQueryDaySettings();
            var regex = new Regex(s.Date);
            var files = GetFiles(Settings.ConnectionString);
            var dates = new Dictionary<DateTime, string>();

            foreach (var file in files)
            {
                var m = regex.Match(file);
                if (m.Success)
                {
                    var d = DateTime.ParseExact(m.Value, s.DateFormat, CultureInfo.InvariantCulture).Date;
                    var f = file;
                    dates.Add(d, f);
                }
            }

            return dates;
        }

        private IEnumerable<string> GetFiles(string dir)
        {
            dir = Environment.ExpandEnvironmentVariables(dir);
            var regex = new Regex(GetQueryDaySettings().File);
            if (Directory.Exists(dir))
            {
                var files = (from f in Directory.GetFiles(dir)
                             where regex.IsMatch(f)
                             select f).ToList();
                return files;
            }
            else { throw new FileNotFoundException($"Directory '{dir}' do not exist. Impossible to find CSV files."); }
        }

        private QueryDaySettings GetQueryDaySettings()
        {
            var d = new QueryDaySettings();
            var lines = (Settings.QueryDay ?? string.Empty).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var file = (from l in lines
                        where l.ToLower().Trim().StartsWith("file")
                        select l).FirstOrDefault();
            var date = (from l in lines
                        where l.ToLower().Trim().StartsWith("date")
                        select l).FirstOrDefault();
            var dateFormat = (from l in lines
                              where l.ToLower().Trim().StartsWith("dateformat")
                              select l).FirstOrDefault();

            d.File = file?.Replace("file:", string.Empty).Trim() ?? "[0-9]{4}-[0-9]{2}-[0-9]{2}\\..*\\.csv";
            d.Date = date?.Replace("date:", string.Empty).Trim() ?? "[0-9]{4}-[0-9]{2}-[0-9]{2}";
            d.DateFormat = dateFormat?.Replace("dateformat:", string.Empty)?.Trim() ?? "yyyy-MM-dd";

            return d;
        }

        private QueryLogSettings GetQueryLogSettings()
        {
            var d = new QueryLogSettings();
            var lines = (Settings.QueryLog ?? string.Empty).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            d.Encoding = (from l in lines
                          where l.ToLower().Trim().StartsWith("encoding")
                          select l)
                            .FirstOrDefault()
                            ?.Replace("encoding:", "").Trim() ?? "windows-1252";

            d.Delimiter = (from l in lines
                           where l.ToLower().Trim().StartsWith("delimiter")
                           select l)
                            .FirstOrDefault()
                            ?.Replace("delimiter:", "").Trim() ?? ";";

            d.Level = (from l in lines
                       where l.ToLower().Trim().StartsWith("level")
                       select l)
                         .FirstOrDefault()
                         ?.Replace("level:", "").Trim() ?? "level";

            d.Logger = (from l in lines
                        where l.ToLower().Trim().StartsWith("logger")
                        select l)
                          .FirstOrDefault()
                          ?.Replace("logger:", "")?.Trim() ?? "logger";

            d.Message = (from l in lines
                         where l.ToLower().Trim().StartsWith("message")
                         select l)
                           .FirstOrDefault()
                           ?.Replace("message:", "")?.Trim() ?? "message";

            d.ThreadId = (from l in lines
                          where l.ToLower().Trim().StartsWith("threadid")
                          select l)
                            .FirstOrDefault()
                            ?.Replace("threadid:", "")?.Trim() ?? "threadid";

            d.Time = (from l in lines
                      where l.ToLower().Trim().StartsWith("time")
                      select l)
                        .FirstOrDefault()
                        ?.Replace("time:", "")?.Trim() ?? "time";

            d.Exception = (from l in lines
                           where l.ToLower().Trim().StartsWith("exception")
                           select l)
                             .FirstOrDefault()
                             ?.Replace("exception:", "")?.Trim() ?? "exception";

            return d;
        }

        private void InitialiseFileWatcher(string path)
        {
            ClearFileWatcher();

            var dir = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);            

            _fw = new FileSystemWatcher(dir)
            {
                EnableRaisingEvents = true,
                Filter = fileName
            };
            _fw.Changed += OnFileChanged;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e) => OnChanged();


        #endregion Methods
    }
}