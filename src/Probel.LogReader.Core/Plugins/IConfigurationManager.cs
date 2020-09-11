using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Filters;
using System;
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

        IAppSettingsDecorator Decorate(AppSettings appSettings);

        void Delete(RepositorySettings repository);

        Task DeleteAsync(RepositorySettings repository);

        AppSettings Get();

        Task<AppSettings> GetAsync();

        IAppSettingsDecorator GetDecorated();

        void Save(AppSettings settings);

        void Save(Action<AppSettings> toSave);

        Task SaveAsync(Action<AppSettings> toSave);

        Task SaveAsync(AppSettings settings);

        Task UpdateAsync(RepositorySettings info);

        #endregion Methods
    }
}