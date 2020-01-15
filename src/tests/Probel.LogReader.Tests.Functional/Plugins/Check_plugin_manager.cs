using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Win32;
using Probel.LogReader.Win32.Files;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Probel.LogReader.Tests.Plugins
{
    public sealed class Check_plugin_manager
    {

        #region Methods

        [Fact]
        public async Task Can_get_plugin_from_configuration()
        {
            var cm = new ConfigurationManager(new MemorySettingsManager());
            IPluginManager mgr = new PluginManager();

            var cfg = new AppSettings();
            cfg.Repositories.Add(new RepositorySettings { PluginId = new Guid("c6d28753-2a41-4e03-a2ab-c9ddcc8652cf") });

            await cm.SaveAsync(cfg);

            var rep = cfg.Repositories[0];
            var plugin = mgr.Build(rep);

            Assert.Equal(new Guid("c6d28753-2a41-4e03-a2ab-c9ddcc8652cf"), rep.PluginId);
            Assert.Equal(10, plugin.GetDays().Count());
        }

        [Fact]
        public void Can_istanciate_debug_plugin()
        {
            IPluginManager mgr = new PluginManager();
            var cfg = new RepositorySettings() { PluginId = new Guid("c6d28753-2a41-4e03-a2ab-c9ddcc8652cf") };

            var plugin = mgr.Build(cfg);

            foreach (var date in plugin.GetDays())
            {
                var logs = plugin.GetLogs(date).Count();
                Assert.Equal(150, logs);
            }
        }

        [Fact]
        public void Can_istanciate_debug_plugin_multiple_times()
        {
            IPluginManager mgr = new PluginManager();

            var rep = new RepositorySettings { PluginId = new Guid("c6d28753-2a41-4e03-a2ab-c9ddcc8652cf") };
            for (var i = 0; i < 3; i++)
            {
                var plugin = mgr.Build(rep);

                foreach (var date in plugin.GetDays())
                {
                    var logs = plugin.GetLogs(date).Count();
                    Assert.Equal(150, logs);
                }
            }
        }

        #endregion Methods
    }
}