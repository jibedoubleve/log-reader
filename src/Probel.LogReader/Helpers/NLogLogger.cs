using NLog;
using NLog.Config;
using System;
using IInternalLogger = Probel.LogReader.Core.Helpers.ILogger;

namespace Probel.LogReader.Helpers
{
    public class NLogLogger : IInternalLogger
    {
        #region Fields

        private Logger _log;

        #endregion Fields

        #region Constructors

        static NLogLogger()
        {
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("InvariantCulture", typeof(InvariantCultureLayoutRendererWrapper));
        }

        public NLogLogger()
        {
            _log = LogManager.GetLogger("Default");
        }

        #endregion Constructors

        #region Methods

        public void Debug(string msg) => _log.Debug(msg);

        public void Error(string msg) => _log.Error(msg);

        public void Error(Exception ex) => _log.Error(ex);

        public void Fatal(string msg) => _log.Fatal(msg);

        public void Trace(string msg) => _log.Trace(msg);

        public void Warn(string msg) => _log.Warn(msg);

        #endregion Methods
    }
}