using Caliburn.Micro;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Ui;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Probel.LogReader.ViewModels
{
    public class EditRepositoryViewModel : Screen
    {
        #region Fields

        public readonly IUserInteraction _user;
        private readonly IConfigurationManager _configManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPluginInfoManager _infoManager;

        private ObservableCollection<PluginInfo> _pluginInfo;
        private RepositorySettings _repository;

        private PluginInfo _selectedPlugin;

        #endregion Fields

        #region Constructors

        public EditRepositoryViewModel(IPluginInfoManager infoManager, IConfigurationManager configManager, IUserInteraction userInteraction, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _user = userInteraction;
            _configManager = configManager;
            _infoManager = infoManager;
        }

        #endregion Constructors

        #region Properties

        public bool CanDeleteRepository => Repository.HasValidId();

        public ObservableCollection<PluginInfo> PluginInfoList
        {
            get => _pluginInfo;
            set => Set(ref _pluginInfo, value, nameof(PluginInfoList));
        }

        public RepositorySettings Repository
        {
            get => _repository;
            set
            {
                if (Set(ref _repository, value, nameof(Repository)))
                {
                    NotifyOfPropertyChange(() => CanDeleteRepository);
                }
            }
        }

        public PluginInfo SelectedPlugin
        {
            get => _selectedPlugin;
            set => Set(ref _selectedPlugin, value, nameof(SelectedPlugin));
        }

        private ManageRepositoryViewModel ParentVm => Parent as ManageRepositoryViewModel;

        #endregion Properties

        #region Methods

        public void Load()
        {
            PluginInfoList = new ObservableCollection<PluginInfo>(_infoManager.GetPluginsInfo());

            SelectedPlugin = (from p in PluginInfoList
                              where p.Id == (Repository?.PluginId ?? new Guid())
                              select p).FirstOrDefault();
        }

        public void RefreshForUpdate() => Repository.PluginId = SelectedPlugin?.Id ?? new Guid();

        protected override void OnDeactivate(bool close) => Repository = new RepositorySettings();

        #endregion Methods
    }
}