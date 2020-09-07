using Caliburn.Micro;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Plugins;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Probel.LogReader.ViewModels
{
    public class EditFilterBindingsViewModel : Screen
    {
        #region Fields

        private readonly IConfigurationManager _configManager;
        private ObservableCollection<FilterSettings> _activeFilters;

        private Guid _id;
        private ObservableCollection<FilterSettings> _inactiveFilters;

        private bool _isDirty;

        #endregion Fields

        #region Constructors

        public EditFilterBindingsViewModel(IConfigurationManager configManager)
        {
            _configManager = configManager;
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<FilterSettings> ActiveFilters
        {
            get => _activeFilters;
            set => Set(ref _activeFilters, value);
        }

        public ObservableCollection<FilterSettings> InactiveFilters
        {
            get => _inactiveFilters;
            set => Set(ref _inactiveFilters, value);
        }

        public bool IsDirty
        {
            get => _isDirty;
            private set => Set(ref _isDirty, value);
        }

        #endregion Properties

        #region Methods

        public void ActivateAll()
        {
            foreach (var item in _inactiveFilters.ToList())
            {
                AddActive(item);
            }
            IsDirty = true;
        }

        public void AddActive(object item)
        {
            if (item is FilterSettings filter)
            {
                _activeFilters.Add(filter);
                _inactiveFilters.Remove(filter);
            }
            IsDirty = true;
        }

        public void AddInactive(object item)
        {
            if (item is FilterSettings filter)
            {
                _inactiveFilters.Add(filter);
                _activeFilters.Remove(filter);
            }
            IsDirty = true;
        }

        public void InactivateAll()
        {
            foreach (var item in _activeFilters.ToList())
            {
                AddInactive(item);
            }
            IsDirty = true;
        }

        public void Load(Guid id)
        {
            _id = id;
            var appSettings = _configManager.GetDecorated();

            var af = appSettings.GetActiveFilters(id);
            ActiveFilters = new ObservableCollection<FilterSettings>(af);

            var ia = appSettings.GetFilters(exept: af);
            InactiveFilters = new ObservableCollection<FilterSettings>(ia);

            IsDirty = false;
        }

        public void Save()
        {
            var appstg = _configManager.GetDecorated();
            appstg.BindFilters(_id, _activeFilters);

            _configManager.Save(appstg.Cast());

            IsDirty = false;
        }

        #endregion Methods
    }
}