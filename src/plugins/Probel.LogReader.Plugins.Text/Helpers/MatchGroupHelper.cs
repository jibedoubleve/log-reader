using System;
using System.Text.RegularExpressions;

namespace Probel.LogReader.Plugins.Text.Helpers
{
    internal static class MatchGroupHelper
    {
        #region Methods

        public static string GetException(this Match match) => match.Get("exception", string.Empty);

        public static string GetLevel(this Match match) => match.Get("level", "info");

        public static string GetLogger(this Match match) => match.Get("logger");

        public static string GetMessage(this Match match) => match.Get("message");

        public static string GetThreadId(this Match match) => match.Get("threadid");

        public static DateTime GetTime(this Match line)
        {
            if (string.IsNullOrEmpty(line.Groups["time"].Value) == false)
            {
                return DateTime.Parse(line.Groups["time"].Value);
            }
            else
            {
                var result = int.TryParse(line.Groups["year"].Value, out var year);
                result &= int.TryParse(line.Groups["month"].Value, out var month);
                result &= int.TryParse(line.Groups["day"].Value, out var day);
                result &= int.TryParse(line.Groups["hour"].Value, out var hour);
                result &= int.TryParse(line.Groups["min"].Value, out var min);
                result &= int.TryParse(line.Groups["sec"].Value, out var sec);
                result &= int.TryParse(line.Groups["msec"].Value, out var msec);

                if (result) { return new DateTime(year, month, day, hour, min, sec, msec); }
                else { throw new NotSupportedException($"Some value is wrong in the regulat expression to select a log row"); }
            }
        }

        private static string Get(this Match m, string item, string replaceBy = null)
        {
            var value = m.Groups[item].Value.TrimEnd('\r', '\n');
            var replaceEmpty = replaceBy != null;

            return replaceEmpty
                     ? (string.IsNullOrEmpty(value) ? replaceBy : value)
                     : value;
        }

        #endregion Methods
    }
}