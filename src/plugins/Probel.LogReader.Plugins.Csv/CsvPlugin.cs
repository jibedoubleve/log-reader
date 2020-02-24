using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Constants;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Plugins.Csv.Config;
using Probel.LogReader.Plugins.Csv.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Probel.LogReader.Plugins.Csv
{
    public class CsvPlugin : PluginBase
    {
        #region Fields

        private readonly string _defaultQueryDay = @"(?<year>[0-9]{4})-(?<month>[0-9]{2})-(?<day>[0-9]{2})\..*\.csv";

        private IEnumerable<LogSource> _dates;
        private FileSystemWatcher _fw;

        #endregion Fields

        #region Properties

        public override bool CanListen => true;

        #endregion Properties

        #region Methods

        public override IEnumerable<DateTime> GetDays(OrderBy orderby = OrderBy.Desc)
        {
            _dates = GetFiles(Settings.ConnectionString);

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
            var s = GetQueryLogSettings();
            var d = (from dd in _dates
                     where dd.Day.Date == day.Date
                     select dd).FirstOrDefault();

            if (d != null)
            {
                var reader = new CsvFileReader(s, d.FilePath);
                return reader.GetLogs(orderby);
            }
            return new List<LogRow>();
        }

        public override void StartListening(DateTime day, int seconds = 0)
        {
            var d = (from dd in _dates
                     where dd.Day.Date == day.Date
                     select dd).FirstOrDefault();

            if (d != null) { InitialiseFileWatcher(d.FilePath); }
            else { throw new NotSupportedException($"No logs found to the specifie day '{day}'"); }
        }

        public override void StopListening() => ClearFileWatcher();

        private DateTime AsDate(Match match)
        {
            int.TryParse(match.Groups["year"].Value, out var year);
            int.TryParse(match.Groups["month"].Value, out var month);
            int.TryParse(match.Groups["day"].Value, out var day);

            try
            {
                return new DateTime(year, month, day);
            }
            catch (Exception) { return DateTime.MaxValue; }
        }

        private void ClearFileWatcher()
        {
            if (_fw != null)
            {
                _fw.Changed -= OnFileChanged;
                _fw = null;
            }
        }

        private IEnumerable<LogSource> GetFiles(string dir)
        {
            dir = Environment.ExpandEnvironmentVariables(dir);

            var regex = new Regex(Settings.QueryDay ?? _defaultQueryDay);

            if (Directory.Exists(dir))
            {
                var files = (from f in Directory.GetFiles(dir)
                             where regex.IsMatch(f)
                             select new LogSource
                             {
                                 Day = AsDate(regex.Match(f)),
                                 FilePath = f
                             }).ToList();
                return files;
            }
            else { throw new FileNotFoundException($"Directory '{dir}' do not exist. Impossible to find CSV files."); }
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