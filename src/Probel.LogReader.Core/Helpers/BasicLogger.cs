using System;
using System.Runtime.CompilerServices;
using T = System.Diagnostics.Trace;

namespace Probel.LogReader.Core.Helpers
{
    public class BasicLogger : ILogger
    {
        #region Methods

        public void Debug(string msg) => Write(msg);

        public void Error(string msg) => Write(msg);

        public void Error(Exception ex) => Write(ex.ToString());

        public void Fatal(string msg) => Write(msg);

        public void Trace(string msg) => Write(msg);

        public void Warn(string msg) => Write(msg);

        private static void Write(string message, [CallerMemberName]  string level = null) => T.WriteLine($"{DateTime.Now:HH:mm:ss.fff} -{level.ToUpper(),-6} - {message}");

        #endregion Methods
    }
}