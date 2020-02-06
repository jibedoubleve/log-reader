using Probel.LogReader.Core.Helpers;
using System;
using System.Runtime.CompilerServices;
using T = System.Diagnostics.Trace;

namespace Probel.LogReader.Helpers
{
    public class BasicLogger : ILogger
    {
        #region Methods

        private static void Write(string message, [CallerMemberName]  string level = null) => T.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")} -{level.ToUpper(),-6} - {message}");

        public void Debug(string msg) => Write(msg);

        public void Error(string msg) => Write(msg);

        public void Error(Exception ex) => Write(ex.ToString());

        public void Fatal(string msg) => Write(msg);

        public void Trace(string msg) => Write(msg);

        public void Warn(string msg) => Write(msg);

        #endregion Methods
    }
}