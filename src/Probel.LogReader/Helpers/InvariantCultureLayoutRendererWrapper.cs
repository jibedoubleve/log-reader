using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using NLog.LayoutRenderers.Wrappers;
using System.Globalization;
using System.Threading;

namespace Probel.LogReader.Helpers
{    /// <remarks>
     /// https://stackoverflow.com/questions/38585028/configure-nlog-to-write-exception-messages-in-english
     /// </remarks>
    [LayoutRenderer("InvariantCulture")]
    [ThreadAgnostic]
    public sealed class InvariantCultureLayoutRendererWrapper : WrapperLayoutRendererBase
    {
        #region Methods

        protected override string RenderInner(LogEventInfo logEvent)
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture;
            try
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                return base.RenderInner(logEvent);
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = currentCulture;
            }
        }

        protected override string Transform(string text)
        {
            return text;
        }

        #endregion Methods
    }
}