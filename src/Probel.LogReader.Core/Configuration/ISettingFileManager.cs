using Probel.LogReader.Core.Configuration;
using System.Threading.Tasks;

namespace Probel.LogReader.Core.Configuration
{
    public interface ISettingFileManager
    {
        #region Methods

        Task<AppSettings> GetAsync();

        Task SaveAsync(AppSettings settings);

        void Delete();
        #endregion Methods
    }
}