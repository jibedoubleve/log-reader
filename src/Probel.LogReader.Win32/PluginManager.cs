using Newtonsoft.Json;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Probel.LogReader.Win32
{
    public class PluginManager : IPluginManager, IPluginInfoManager
    {
        #region Fields

        private const string _pluginRepository = @"%appdata%\probel\log-reader\plugins\";
        private readonly Dictionary<string, Type> _pluginTypes = new Dictionary<string, Type>();
        private List<IPluginMetadata> _metadataList;

        #endregion Fields

        #region Constructors

        public PluginManager()
        {
            PluginRepository = Environment.ExpandEnvironmentVariables(_pluginRepository);
        }

        #endregion Constructors

        #region Properties

        private string PluginRepository { get; }

        #endregion Properties

        #region Methods

        public PluginBase Build(RepositorySettings settings)
        {
            LoadPlugins();

            var metadata = (from m in _metadataList
                            where m.PluginId == settings.PluginId
                            select m).SingleOrDefault();

            if (metadata == null) { return new EmptyPlugin(); }
            else if (_pluginTypes.ContainsKey(metadata.Dll))
            {
                var type = _pluginTypes[metadata.Dll];
                var plugin = Activator.CreateInstance(type);

                var p = plugin as PluginBase;
                p?.Initialise(settings);
                return p;
            }
            else { throw new NotSupportedException($"Cannot instanciate the plugin '{metadata.Dll}'. Did you forget to load the plugins?"); }
        }

        public IEnumerable<PluginInfo> GetPluginsInfo()
        {
            LoadPlugins();

            var result = (from m in _metadataList ?? new List<IPluginMetadata>()
                          select new PluginInfo(m.PluginId, m.Name, m.DocUrl, m.Description));
            return result;
        }

        private void Load(string dir, string dll)
        {
            var path = Path.Combine(dir, dll);
            if (File.Exists(path) == false) { throw new ArgumentException($"Cannot load plugin's file '{path}', file does not exist."); }
            else if (_pluginTypes.ContainsKey(dll) == false)
            {
                try
                {
                    var asmPath = AssemblyName.GetAssemblyName(path);
                    var assembly = Assembly.Load(asmPath);
                    var type = (from t in assembly.GetTypes()
                                where t.IsClass
                                   && !t.IsAbstract
                                   && t.GetInterfaces().Contains(typeof(IPlugin))
                                select t).First();

                    _pluginTypes.Add(dll, type);
                }
                catch (InvalidOperationException ex) { throw new InvalidOperationException($"An error occured when searching 'Plugin' class for dll '{dll}'", ex); }
            }
        }

        private void LoadPlugins()
        {
            if (_metadataList == null)
            {
                _metadataList = new List<IPluginMetadata>();

                foreach (var file in Directory.EnumerateFiles(PluginRepository, "plugin.config.json", SearchOption.AllDirectories))
                {
                    var json = File.ReadAllText(file);
                    var metadata = JsonConvert.DeserializeObject<PluginMetadata>(json);
                    _metadataList.Add(metadata);

                    var dir = Path.GetDirectoryName(file);
                    Load(dir, metadata.Dll);
                }
            }
        }

        #endregion Methods
    }
}