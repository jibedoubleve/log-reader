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
            CreateRepositoryCommand = new RelayCommand(CreateRepository);
            SaveAllCommand = new RelayCommand(SaveAll);

            _userInteraction = userInteraction;
            _eventAggregator = eventAggregator;
            _editRepositoryViewModel = editRepositoryViewModel;
            _configManager = configManager;
        }

        #endregion Constructors

        #region Properties

        public ICommand CreateRepositoryCommand { get; private set; }

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

        public ICommand SaveAllCommand { get; private set; }

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

        public void DiscardAll()
        {
            if (_userInteraction.Ask(Strings.Msg_AskReset) == UserAnswers.Yes)
            {
                Load();
            }
        }

        public void Load()
        {
            //TODO: check, seems to works by accident!
            var t1 = Task.Run(() => _cachedAppSettings = _configManager.Get());
            t1.OnErrorHandle(_userInteraction);

            var t2 = t1.ContinueWith(r => Repositories = new ObservableCollection<RepositorySettings>(_cachedAppSettings.Repositories), TaskContinuationOptions.OnlyOnRanToCompletion);
            t2.OnErrorHandle(_userInteraction);
        }

        public void SaveAll()
        {
            _editRepositoryViewModel.RefreshForUpdate();

            var t1 = Task.Run(() =>
            {
                _configManager.Save(_cachedAppSettings);

                _eventAggregator.PublishOnBackgroundThread(UiEvent.RefreshMenus);
                _userInteraction.NotifySuccess(Strings.Msg_InformRepositorySaved);
            });
            t1.OnErrorHandle(_userInteraction);
        }

        private void DeleteCurrentRepository()
        {
            if (_userInteraction.Ask(Strings.Msg_AskDelete) == UserAnswers.Yes)
            {
                var d = _configManager.Decorate(_cachedAppSettings);
                d.Remove(CurrentRepository);

                Repositories = new ObservableCollection<RepositorySettings>(d.Cast().Repositories);
            }
        }

        #endregion Methods
    }
}