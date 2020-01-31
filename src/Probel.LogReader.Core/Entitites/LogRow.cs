using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Probel.LogReader.Core.Configuration
{
    internal static class LogRowExtension
    {
        #region Methods

        //Todo: check, can be buggy code ;-)
        public static int GetId(this LogRow src)
        {
            var str = src.Exception + src.Level + src.Logger + src.Message + src.ThreadId + src.ToString();
            return str.GetHashCode();
        }

        #endregion Methods
    }
    [DebuggerDisplay("{Time} | [{Level}] - {Logger}")]
    public class LogRow
    {
        #region Properties

        public string Exception { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string ThreadId { get; set; }
        public DateTime Time { get; set; }

        #endregion Properties
    }

    public class LogRowEqualityComparer : IEqualityComparer<LogRow>
    {
        #region Methods

        public bool Equals(LogRow x, LogRow y) => x.GetId() == y.GetId();

        public int GetHashCode(LogRow obj) => obj.GetId();

        #endregion Methods
    }
}