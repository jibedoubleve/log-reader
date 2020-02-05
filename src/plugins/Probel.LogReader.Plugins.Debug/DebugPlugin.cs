using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Constants;
using Probel.LogReader.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Probel.LogReader.Plugins.Debug
{
    public class DebugPlugin : PluginBase
    {
        #region Fields

        private readonly List<DateTime> _dates = new List<DateTime>();
        private readonly List<LogRow> _logs = new List<LogRow>();
        private readonly Random _random = new Random();
        private bool _doClear;

        private List<LogRow> _tempLogRows = null;

        #endregion Fields

        #region Constructors

        public DebugPlugin()
        {
            for (var i = 0; i < 10; i++)
            {
                var date = DateTime.Today.AddDays(0 - i).Date;
                var logs = BuildRandomLogs(150, date);
                _logs.AddRange(logs);
                _dates.Add(date);
            }
        }

        #endregion Constructors

        #region Methods

        public DebugPlugin Add(params LogRow[] logRow)
        {
            if (_tempLogRows == null) { _tempLogRows = new List<LogRow>(); }
            _tempLogRows.AddRange(logRow);
            return this;
        }

        public void AddMinutes(int minutes)
        {
            var now = DateTime.Now.AddMinutes(minutes);
            foreach (var item in _logs) { item.Time = now; }

            _dates.Clear();
            _dates.Add(now.Date);
        }

        public DebugPlugin Clear()
        {
            _doClear = true;
            return this;
        }

        public void Commit()
        {
            if (_doClear) { _logs.Clear(); }
            if (_tempLogRows != null) { _logs.AddRange(_tempLogRows); }

            var dates = (from l in _logs
                         select l.Time.Date).Distinct();
            _dates.Clear();
            _dates.AddRange(dates);
        }

        public override IEnumerable<DateTime> GetDays(OrderBy orderby = OrderBy.Desc)
        {
            switch (orderby)
            {
                case OrderBy.Asc: return _dates.OrderBy(e => e);
                case OrderBy.Desc: return _dates.OrderByDescending(e => e);
                case OrderBy.None: return _dates;
                default: throw new NotSupportedException($"This sort '{orderby}' is not supported!");
            }
        }

        public override IEnumerable<LogRow> GetLogs(DateTime day, OrderBy orderby = OrderBy.Desc)
        {
            var logs = (from l in _logs
                        where l.Time.Date == day.Date
                        select l);
            switch (orderby)
            {
                case OrderBy.Asc:
                    return logs.OrderBy(e => e.Time);

                case OrderBy.Desc:
                    return logs.OrderBy(e => e.Time);

                case OrderBy.None:
                    return logs;

                default:
                    throw new NotSupportedException($"The order by clause '{orderby}' is not supported!");
            }
        }

        public void SetCategory(string category)
        {
            foreach (var item in _logs) { item.Logger = category; }
        }

        public void SetLevel(string level)
        {
            foreach (var item in _logs) { item.Level = level; }
        }

        private List<LogRow> BuildRandomLogs(int logCount, DateTime date)
        {
            var logs = new List<LogRow>();
            for (var i = 0; i < logCount; i++)
            {
                var b = _random.Next(-1, 1) == 0;
                logs.Add(new LogRow()
                {
                    Exception = b ? new Exception().ToString() : null,
                    Level = GetLevel(),
                    Logger = GetLogger(),
                    Message = Guid.NewGuid().ToString(),
                    ThreadId = _random.Next(0, 150).ToString(),
                    Time = date.Date.AddMinutes(_random.Next(1439))
                });
            }

            return logs;
        }

        private string GetLevel()
        {
            var lvl = _random.Next(1, 6);
            switch (lvl)
            {
                case 1: return "trace";
                case 2: return "debug";
                case 3: return "info";
                case 4: return "warn";
                case 5: return "error";
                case 6: return "fatal";
                default: throw new NotSupportedException($"Unsupported level {lvl}");
            }
        }

        private string GetLogger()
        {
            var lvl = _random.Next(1, 6);
            switch (lvl)
            {
                case 1: return "Logger one";
                case 2: return "Logger two";
                case 3: return "Logger default";
                case 4: return "Logger four";
                case 5: return "Logger five";
                case 6: return "Logger six";
                default: throw new NotSupportedException($"Unsupported level {lvl}");
            }
        }

        #endregion Methods
    }
}