using Caliburn.Micro;
using Probel.LogReader.Core.Plugins;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Probel.LogReader.ViewModels
{
    public class DaysViewModel : Screen
    {
        #region Fields

        private ObservableCollection<DateTime> _days;

        private DateTime _selectedDay;

        #endregion Fields

        #region Properties

        public ObservableCollection<DateTime> Days
        {
            get => _days;
            set => Set(ref _days, value, nameof(Days));
        }

        public PluginBase Plugin { get; set; }

        public DateTime SelectedDay
        {
            get => _selectedDay;
            set => Set(ref _selectedDay, value, nameof(SelectedDay));
        }

        #endregion Properties

        #region Methods

        public async Task LoadLogsAsync()
        {
            if (Parent is MainViewModel parent)
            {
                var logs = Plugin.GetLogs(SelectedDay);
                await parent.ActivateLogsAsync(logs);
            }
        }

        #endregion Methods
    }
}