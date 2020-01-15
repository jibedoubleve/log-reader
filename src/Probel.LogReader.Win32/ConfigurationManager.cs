using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Filters;
using Probel.LogReader.Core.Plugins;
using System.Linq;
using System.Threading.Tasks;

namespace Probel.LogReader.Win32
{
    public class ConfigurationManager : IConfigurationManager
    {
        #region Fields

        private readonly ISettingFileManager _fileManager;

        #endregion Fields

        #region Constructors

        public ConfigurationManager(ISettingFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        #endregion Constructors

        #region Methods

        public async Task<IFilterManager> BuildFilterManagerAsync()
        {
            var cfg = await GetAsync();
            var fm = new FilterManager(cfg.Filters);
            return fm;
        }

        public async Task CreateAsync(RepositorySettings repository)
        {
            var a = await GetAsync();
            a.Repositories.Add(repository);
            await SaveAsync(a);
        }

        public void Delete() => _fileManager.Delete();

        public async Task DeleteAsync(RepositorySettings repository)
        {
            var a = await GetAsync();
            var del = a.Repositories.Where(e => e.PluginId == repository.PluginId).FirstOrDefault();
            if (del != null)
            {
                a.Repositories.Remove(del);
                await SaveAsync(a);
            }
        }

        public async Task<AppSettings> GetAsync()
        {
            var app = await _fileManager.GetAsync();
            if (app.Filters.Where(e => e.Id == FilterSettings.NoFilter.Id).Count() == 0)
            {
                app.Filters.Add(FilterSettings.NoFilter);
            }
            return app;
        }

        public async Task SaveAsync(AppSettings settings) => await _fileManager.SaveAsync(settings);

        public async Task UpdateAsync(RepositorySettings info)
        {
            var a = await GetAsync();
            var del = a.Repositories.Where(e => e.PluginId == info.PluginId).FirstOrDefault();
            if (del != null)
            {
                a.Repositories.Remove(del);
                a.Repositories.Add(info);
                await SaveAsync(a);
            }
        }

        #endregion Methods
    }
}