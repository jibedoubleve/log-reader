using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Plugins.Debug;
using System.Linq;
using Xunit;

namespace Probel.LogReader.TestCases.Plugins
{
    public class Check_plugins
    {
        #region Methods

        [Fact]
        public void Has_days()
        {
            PluginBase plugin = new DebugPlugin();

            var days = plugin.GetDays();

            Assert.True(days.Count() > 0);
        }

        [Fact]
        public void Has_expected_logs_for_day()
        {
            PluginBase plugin = new DebugPlugin();

            var days = plugin.GetDays();
            foreach (var day in days)
            {
                var logs = plugin.GetLogs(day);
                Assert.Equal(150, logs.Count());
            }
        }

        #endregion Methods
    }
}