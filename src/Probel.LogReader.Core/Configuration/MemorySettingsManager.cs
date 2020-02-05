using System.Threading.Tasks;

namespace Probel.LogReader.Core.Configuration
{
    public class MemorySettingsManager : ISettingFileManager
    {
        #region Fields

        private AppSettings _settings = new AppSettings();

        #endregion Fields

        #region Methods

        public void Delete() => _settings = null;

        public AppSettings Get() => _settings;

        public Task<AppSettings> GetAsync() => Task.FromResult(_settings);

        public void Save(AppSettings settings) => _settings = settings;

        public Task SaveAsync(AppSettings settings)
        {
            _settings = settings;
            return Task.CompletedTask;
        }

        #endregion Methods
    }
}