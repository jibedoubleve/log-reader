using System.Threading.Tasks;

namespace Probel.LogReader.Core.Configuration
{
    public interface ISettingFileManager
    {
        #region Methods

        void Delete();

        AppSettings Get();

        Task<AppSettings> GetAsync();

        void Save(AppSettings settings);

        Task SaveAsync(AppSettings settings);

        #endregion Methods
    }
}