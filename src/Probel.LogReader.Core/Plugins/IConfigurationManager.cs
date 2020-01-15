using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Filters;
using System.Threading.Tasks;

namespace Probel.LogReader.Core.Plugins
{
    public interface IConfigurationManager
    {
        #region Methods

        Task<IFilterManager> BuildFilterManagerAsync();

        Task CreateAsync(RepositorySettings repository);

        Task DeleteAsync(RepositorySettings repository);

        Task<AppSettings> GetAsync();

        Task SaveAsync(AppSettings settings);

        Task UpdateAsync(RepositorySettings info);

        #endregion Methods
    }
}