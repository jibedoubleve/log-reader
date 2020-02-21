using System;

namespace Probel.LogReader.Core.Helpers
{
    public interface ILogger
    {
        #region Methods

        void Debug(string msg);

        void Error(string msg);

        void Error(Exception ex);

        void Fatal(string msg);

        void Trace(string msg);

        void Warn(string msg);

        #endregion Methods
    }
}