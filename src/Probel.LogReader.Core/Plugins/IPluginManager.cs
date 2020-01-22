using Probel.LogReader.Core.Configuration;

namespace Probel.LogReader.Core.Plugins
{

    public interface IPluginManager
    {
        #region Methods

        PluginBase Build(RepositorySettings cfg);

        #endregion Methods
    }
}