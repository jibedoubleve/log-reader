using Probel.LogReader.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Probel.LogReader.Plugins.Text.Helpers
{
    internal partial class FileFinder
    {
        #region Fields

        private Query _query;
        private Regex _regex = null;

        #endregion Fields

        #region Constructors

        public FileFinder(string queryDay)
        {
            if (Query.Contains(queryDay.ToLower()))
            {
                _query = new Query(queryDay);
            }
            else { _regex = new Regex(queryDay); }
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<LogSource> GetFiles(string dir)
        {
            return _regex != null
                ? GetFilesOnRegEx(dir)
                : GetFileOnProperty(dir);
        }

        private static IEnumerable<LogSource> GetFileOnLastUpdate(string dir)
        {
            return (from f in Directory.GetFiles(dir)
                    select new LogSource
                    {
                        Day = File.GetLastWriteTime(f).Date,
                        FilePath = f
                    });
        }

        private IEnumerable<LogSource> GetFileOnCreationDate(string dir)
        {
            return (from f in Directory.GetFiles(dir)
                    select new LogSource
                    {
                        Day = File.GetCreationTime(f).Date,
                        FilePath = f
                    });
        }

        private IEnumerable<LogSource> GetFileOnProperty(string dir)
        {
            if (_query.IsOnLastWrite) { return GetFileOnLastUpdate(dir); }
            else if (_query.IsOnCreatioDate) { return GetFileOnCreationDate(dir); }
            else { throw new NotSupportedException($"The query '{_query.Value}'"); }
        }

        private IEnumerable<LogSource> GetFilesOnRegEx(string dir)
        {
            var files = (from f in Directory.GetFiles(dir)
                         where _regex.IsMatch(f)
                         select new LogSource
                         {
                             Day = _regex.Match(f).AsDate(),
                             FilePath = f
                         }).ToList();
            return files;
        }

        #endregion Methods
    }
}