using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Filters;
using System.Threading.Tasks;

namespace Probel.LogReader.Core.Plugins
{
    public interface IConfigurationManager
    {
        #region Methods

        IFilterManager BuildFilterManager();

        Task<IFilterManager> BuildFilterManagerAsync();

        void Create(RepositorySettings repository);

        Task CreateAsync(RepositorySettings repository);

        void Delete(RepositorySettings repository);

        Task DeleteAsync(RepositorySettings repository);

        AppSettings Get();

        Task<AppSettings> GetAsync();

        void Save(AppSettings settings);

        Task SaveAsync(AppSettings settings);

        Task UpdateAsync(RepositorySettings info);

        #endregion Methods
    }
}