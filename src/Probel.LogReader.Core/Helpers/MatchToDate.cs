using System;
using System.Text.RegularExpressions;

namespace Probel.LogReader.Core.Helpers
{
    public static class MatchToDate
    {
        #region Methods

        /// <summary>
        /// Parse the regex matches.
        /// </summary>
        /// <param name="match"></param>
        /// <returns>The parsed <see cref="DateTime"/></returns>
        public static DateTime AsDate(this Match match)
        {
            int.TryParse(match.Groups["year"].Value, out var year);
            int.TryParse(match.Groups["month"].Value, out var month);
            int.TryParse(match.Groups["day"].Value, out var day);

            return new DateTime(year, month, day);
        }

        #endregion Methods
    }
}