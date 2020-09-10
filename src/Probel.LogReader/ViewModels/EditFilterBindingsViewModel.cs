using Caliburn.Micro;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Ui;
using System.Collections.ObjectModel;
using System.Linq;

namespace Probel.LogReader.ViewModels
{
    public class EditFilterBindingsViewModel : Screen
    {
        #region Fields

        private readonly IConfigurationManager _configManager;
        private readonly IUserInteraction _userInteraction;
        private ObservableCollection<FilterSettings> _activeFilters = new ObservableCollection<FilterSettings>();
        private ObservableCollection<FilterSettings> _inactiveFilters = new ObservableCollection<FilterSettings>();
        private bool _isDirty;
        private RepositorySettings _repository;

        #endregion Fields

        #region Constructors

        public EditFilterBindingsViewModel(
            IConfigurationManager configManager,
            IUserInteraction userInteraction)
        {
            _userInteraction = userInteraction;
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

        public void Load(RepositorySettings repository)
        {
            _repository = repository;
            var appSettings = _configManager.GetDecorated();

            var af = appSettings.GetActiveFilters(repository.Id);
            ActiveFilters = new ObservableCollection<FilterSettings>(af);

            var ia = appSettings.GetFilters(exept: af);
            InactiveFilters = new ObservableCollection<FilterSettings>(ia);

            IsDirty = false;
        }

        public void Save()
        {
            if (_repository != null)
            {
                var appstg = _configManager.GetDecorated();
                appstg.BindFilters(_repository.Id, _activeFilters);

                _configManager.Save(appstg.Cast());

                _userInteraction.NotifyInformation($"Linked {_activeFilters.Count()} filter(s) to repository {_repository.Name}.");

                IsDirty = false;
            }
            else
            {
                _userInteraction.NotifyInformation("Nothing to save.");
            }
        }

        #endregion Methods
    }
}