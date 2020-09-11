using Caliburn.Micro;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Helpers;
using Probel.LogReader.Ui;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Probel.LogReader.ViewModels
{
    public class ManageFilterBindingsViewModel : Conductor<IScreen>
    {
        #region Fields

        private readonly IConfigurationManager _configManager;
        private readonly EditFilterBindingsViewModel _editFilterBindingsViewModel;
        private readonly IUserInteraction _userInteraction;
        private AppSettings _cachedAppSettings;
        private RepositorySettings _currentRepository;
        private ObservableCollection<RepositorySettings> _repositories;

        #endregion Fields

        #region Constructors

        public ManageFilterBindingsViewModel(
            IConfigurationManager configManager,
            IUserInteraction userInteraction,
            EditFilterBindingsViewModel editFilterBindingsViewModel)
        {
            _editFilterBindingsViewModel = editFilterBindingsViewModel;
            _userInteraction = userInteraction;
            _configManager = configManager;
        }

        #endregion Constructors

        #region Properties

        public RepositorySettings CurrentRepository
        {
            get => _currentRepository;
            set => Set(ref _currentRepository, value, nameof(CurrentRepository));
        }

        public ObservableCollection<RepositorySettings> Repositories
        {
            get => _repositories;
            set => Set(ref _repositories, value, nameof(Repositories));
        }

        #endregion Properties

        #region Methods

        public void ActivateAll() => _editFilterBindingsViewModel.ActivateAll();

        public void ActivateCurrentRepository()
        {
            if (_editFilterBindingsViewModel.IsDirty)
            {
                var result = _userInteraction.Ask("Do you want to save?", "Quit");

                if (result == UserAnswers.Yes) { SaveAll(); }
                else { Load(); }
            }

            DeactivateItem(_editFilterBindingsViewModel, true);

            if (CurrentRepository != null)
            {
                _editFilterBindingsViewModel.Load(CurrentRepository);
                ActivateItem(_editFilterBindingsViewModel);
            }
        }

        public void InactivateAll() => _editFilterBindingsViewModel.InactivateAll();

        public void Load()
        {
            var t1 = Task.Run(() => _cachedAppSettings = _configManager.Get());
            t1.OnErrorHandle(_userInteraction);

            var t2 = t1.ContinueWith(r => Repositories = new ObservableCollection<RepositorySettings>(_cachedAppSettings.Repositories), TaskContinuationOptions.OnlyOnRanToCompletion);
            t2.OnErrorHandle(_userInteraction);
        }

        public void SaveAll() => _editFilterBindingsViewModel.Save();

        #endregion Methods
    }
}