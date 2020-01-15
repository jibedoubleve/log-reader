using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Configuration;
using System.Threading.Tasks;

namespace Probel.LogReader.Win32.Files
{
    public class MemorySettingsManager : ISettingFileManager
    {
        #region Fields

        private AppSettings _settings = new AppSettings();

        #endregion Fields

        #region Methods

        public void Delete() => _settings = null;

        public Task<AppSettings> GetAsync() => Task.FromResult(_settings);

        public Task SaveAsync(AppSettings settings)
        {
            _settings = settings;
            return Task.CompletedTask;
        }

        #endregion Methods
    }
}