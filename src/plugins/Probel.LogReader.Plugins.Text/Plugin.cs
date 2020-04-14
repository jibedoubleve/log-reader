using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Constants;
using Probel.LogReader.Core.Helpers;
using Probel.LogReader.Core.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Probel.LogReader.Plugins.Text
{
    public class Plugin : PluginBase
    {
        #region Fields

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
            var path = (from s in _dates
                        where s.Day.Date == day.Date
                        select s.FilePath).FirstOrDefault();
            try
            {
                var extractor = new LogExtractor(Settings.QueryLog);

                return extractor.Extract(path);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Cannot get logs from file '{path}'", ex);
            }
        }

        public override void StartListening(DateTime day, int seconds = 0)
        {
            var d = (from dd in _dates
                     where dd.Day.Date == day.Date
                     select dd).FirstOrDefault();

            if (d != null) { InitialiseFileWatcher(d.FilePath); }
            else { Logger.Warn($"No logs found to the specified day '{day}'"); }
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