using Caliburn.Micro;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Helpers;
using Probel.LogReader.Properties;
using Probel.LogReader.Ui;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Probel.LogReader.ViewModels
{
    public class ManageRepositoryViewModel : Conductor<IScreen>
    {
        #region Fields

        public readonly IUserInteraction _userInteraction;
        private readonly IConfigurationManager _configManager;
        private readonly EditRepositoryViewModel _editRepositoryViewModel;
        private readonly IEventAggregator _eventAggregator;
        private AppSettings _cachedAppSettings;
        private RepositorySettings _currentRepositorySettings;
        private ObservableCollection<RepositorySettings> _repositories;

        #endregion Fields

        #region Constructors

        public ManageRepositoryViewModel(IConfigurationManager configManager
            , EditRepositoryViewModel editRepositoryViewModel
            , IEventAggregator eventAggregator
            , IUserInteraction userInteraction)
        {
            DeleteCurrentRepositoryCommand = new RelayCommand(DeleteCurrentRepository);
            _userInteraction = userInteraction;
            _eventAggregator = eventAggregator;
            _editRepositoryViewModel = editRepositoryViewModel;
            _configManager = configManager;
        }

        #endregion Constructors

        #region Properties

        public RepositorySettings CurrentRepository
        {
            get => _currentRepositorySettings;
            set => Set(ref _currentRepositorySettings, value, nameof(CurrentRepository));
        }

        public ICommand DeleteCurrentRepositoryCommand { get; private set; }

        public ObservableCollection<RepositorySettings> Repositories
        {
            get => _repositories;
            set => Set(ref _repositories, value, nameof(Repositories));
        }

        #endregion Properties

        #region Methods

        public void ActivateCurrentRepository()
        {
            if (CurrentRepository != null)
            {
                _editRepositoryViewModel.Repository = CurrentRepository;
                _editRepositoryViewModel.Load();                
                ActivateItem(_editRepositoryViewModel);
            }
        }

        public void CreateRepository()
        {
            var newItem = new RepositorySettings() { Name = "New repository" };
            CurrentRepository = newItem;

            Repositories.Add(newItem);
            _cachedAppSettings.Repositories.Add(newItem);

            ActivateItem(_editRepositoryViewModel);
        }

        //TODO: Error handling
        public async void DiscardAll()
        {
            if (_userInteraction.Ask(Strings.Msg_AskReset) == UserAnswers.Yes)
            {
                await LoadAsync();
            }
        }

        public async Task LoadAsync()
        {
            _cachedAppSettings = await _configManager.GetAsync();
            Repositories = new ObservableCollection<RepositorySettings>(_cachedAppSettings.Repositories);
        }

        //TODO: Error handling
        public async void SaveAll()
        {
            _editRepositoryViewModel.RefreshForUpdate();
            await _configManager.SaveAsync(_cachedAppSettings);
            _eventAggregator.PublishOnBackgroundThread(UiEvent.RefreshMenus);

            _userInteraction.Inform(Strings.Msg_InformSaved);
        }

        private void DeleteCurrentRepository()
        {
            if (_userInteraction.Ask(Strings.Msg_AskDelete) == UserAnswers.Yes)
            {
                _cachedAppSettings.Repositories.Remove(CurrentRepository);
                Repositories.Remove(CurrentRepository);
            }
        }

        #endregion Methods
    }
}