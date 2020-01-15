using Probel.LogReader.Core.Configuration;

namespace Probel.LogReader.Core.Plugins
{

    public interface IPluginManager
    {
        #region Methods

        IPlugin Build(RepositorySettings cfg);

        #endregion Methods
    }
}