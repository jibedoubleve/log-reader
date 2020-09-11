using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Core.Plugins.Loaders;
using Probel.LogReader.Plugins.Debug;
using System;
using System.Collections.Generic;

namespace Probel.LogReader.Tests.Helpers
{
    public class DebugLoader : IPluginLoader
    {
        #region Methods

        public IList<IPluginMetadata> LoadPlugins(string pluginRepository, Dictionary<string, Type> pluginTypes)
        {
            var metadata = new PluginMetadata
            {
                Colouration = null,
                Description = "DEBUG plugin",
                Dll = "Probel.LogReader.Plugins.Debug.dll",
                DocUrl = string.Empty,
                Name = "DEBUG",
                PluginId = new Guid("c6d28753-2a41-4e03-a2ab-c9ddcc8652cf"),
            };
            var metadataList = new List<IPluginMetadata> { metadata };

            if (pluginTypes.ContainsKey(metadata.Dll) == false)
            {
                pluginTypes.Add("Probel.LogReader.Plugins.Debug.dll", typeof(Plugin));
            }
            return metadataList;
        }

        #endregion Methods
    }
}