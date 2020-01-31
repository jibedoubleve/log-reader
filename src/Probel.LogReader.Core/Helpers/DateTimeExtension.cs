using System;

namespace Probel.LogReader.Core.Helpers
{
    public static class DateTimeExtension
    {
        #region Methods

        public static DateTime TrimSeconds(this DateTime s) => new DateTime(s.Year, s.Month, s.Day, s.Hour, s.Minute, 0, 0);

        #endregion Methods
    }
}