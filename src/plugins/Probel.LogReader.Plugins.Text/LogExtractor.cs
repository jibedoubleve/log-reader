﻿using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Plugins.Text.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Probel.LogReader.Plugins.Text
{
    public class LogExtractor
    {
        #region Fields

        private Regex _regex;

        #endregion Fields

        #region Constructors

        public LogExtractor(string pattern)
        {
            _regex = new Regex(pattern);
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<LogRow> Extract(string path)
        {
            var result = new List<LogRow>();
            if (string.IsNullOrEmpty(path) == false)
            {
                var content = File.ReadAllText(path);
                var lines = _regex.Matches(content);

                foreach (Match line in lines)
                {
                    result.Add(GetRow(line));

                }
            }
            return result;
        }

        private LogRow GetRow(Match line)
        {
            if (line.Groups.Count == 5)
            {
                return new LogRow
                {
                    Time = DateTime.Parse(line.Groups["time"].Value),
                    Level = line.GetLevel(),
                    Logger = line.GetLogger(),
                    Exception = line.GetException(),
                    Message = line.GetMessage(),
                    ThreadId = line.GetThreadId(),
                };
            }
            else
            {
                return new LogRow
                {
                    Message = line.Groups[0].Value
                };
            }

        }

        #endregion Methods
    }
}