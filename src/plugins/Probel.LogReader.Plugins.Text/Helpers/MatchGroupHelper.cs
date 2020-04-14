using System.Text.RegularExpressions;

namespace Probel.LogReader.Plugins.Text.Helpers
{
    public static class MatchGroupHelper
    {
        #region Methods

        public static string GetException(this Match match) => match.Get("exception", string.Empty);

        public static string GetLevel(this Match match) => match.Get("level", "info");

        public static string GetLogger(this Match match) => match.Get("logger");

        public static string GetMessage(this Match match) => match.Get("message");

        public static string GetThreadId(this Match match) => match.Get("threadid");

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