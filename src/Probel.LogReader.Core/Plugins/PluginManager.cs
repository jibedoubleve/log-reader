using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Helpers;
using Probel.LogReader.Core.Plugins.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Probel.LogReader.Core.Plugins
{
    public class PluginManager : IPluginManager, IPluginInfoManager
    {
        #region Fields

        private const string _pluginRepository = @"%appdata%\probel\log-reader\plugins\";
        private readonly ILogger _logger;
        private readonly IPluginLoader _pluginLoader;
        private readonly Dictionary<string, Type> _pluginTypes = new Dictionary<string, Type>();
        private IList<IPluginMetadata> _metadataList;

        #endregion Fields

        #region Constructors

        public PluginManager(IPluginLoader loader, ILogger logger)
        {
            _logger = logger;
            _pluginLoader = loader;
        }

        #endregion Constructors

        #region Methods

        public PluginBase Build(RepositorySettings settings)
        {
            _metadataList = _pluginLoader.LoadPlugins(_pluginRepository, _pluginTypes);

            var metadata = (from m in _metadataList
                            where m.PluginId == settings.PluginId
                            select m).SingleOrDefault();

            if (metadata == null) { return new EmptyPlugin(); }
            else if (_pluginTypes.ContainsKey(metadata.Dll))
            {
                var type = _pluginTypes[metadata.Dll];
                var plugin = Activator.CreateInstance(type);

                var p = plugin as PluginBase;
                p?.Initialise(settings, _logger);
                return p;
            }
            else { throw new NotSupportedException($"Cannot instanciate the plugin '{metadata.Dll}'. Did you forget to load the plugins?"); }
        }

        public IEnumerable<PluginInfo> GetPluginsInfo()
        {
            _metadataList = _pluginLoader.LoadPlugins(_pluginRepository, _pluginTypes);

            var result = (from m in _metadataList ?? new List<IPluginMetadata>()
                          select new PluginInfo(m.PluginId, m.Name, m.DocUrl, m.Description, m.Colouration));
            return result;
        }

        #endregion Methods
    }
}