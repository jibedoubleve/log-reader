using System.Text.RegularExpressions;

namespace Probel.LogReader.Plugins.Text.Helpers
{
    public static class MatchGroupHelper
    {
        #region Methods
        public static string GetException(this Match match) => match.Get("exception", false);

        public static string GetLevel(this Match match) => match.Get("level");

        public static string GetLogger(this Match match) => match.Get("logger");

        public static string GetMessage(this Match match) => match.Get("message");

        public static string GetThreadId(this Match match) => match.Get("threadid");

        private static string Get(this Match m, string item, bool replaceEmpty = true)
        {
            var value = m.Groups[item].Value.TrimEnd('\r', '\n');

            return replaceEmpty
                ? (string.IsNullOrEmpty(value) ? "<N.A.>" : value)
                : value;
        }

        #endregion Methods
    }
}