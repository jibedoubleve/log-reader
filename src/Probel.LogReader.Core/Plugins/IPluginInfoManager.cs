using System.Collections.Generic;

namespace Probel.LogReader.Core.Plugins
{
    public interface IPluginInfoManager
    {
        #region Methods

        IEnumerable<PluginInfo> GetPluginsInfo();

        #endregion Methods
    }
}