using Caliburn.Micro;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Filters;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Helpers;
using Probel.LogReader.Models;
using Probel.LogReader.Ui;
using Probel.LogReader.ViewModels.Packs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
namespace Probel.LogReader.ViewModels
{
    public class MainViewModel : Conductor<IScreen>, IHandleWithTask<UiEvent>
    {
        #region Fields

        private readonly IConfigurationManager _configurationManager;
        private readonly IPluginInfoManager _pluginInfoManager;
        private readonly ManageFilterViewModel _manageFilterViewModel;
        private readonly IPluginManager _pluginManager;

        private readonly DaysViewModel _vmDaysViewModel;
        private readonly LogsViewModel _vmLogsViewModel;

        private bool _isFilterVisible = false;
        private ObservableCollection<MenuItemModel> _menuFile;
        private ObservableCollection<MenuItemModel> _menuFilter;

        #endregion Fields

        #region Constructors

        public MainViewModel(IConfigurationManager cfg
            , IPluginInfoManager pluginInfoManager
            , IPluginManager pluginManager
            , IFilterTranslator filterTranslator
            , MainViewModelPack views
            , IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);

            _configurationManager = cfg;

            _pluginInfoManager = pluginInfoManager;
            _pluginManager = pluginManager;
            _filterTranslator = filterTranslator;
            _vmDaysViewModel = views.DaysViewModel;
            _vmLogsViewModel = views.LogsViewModel;
            _manageRepositoryViewModel = views.ManageRepositoryViewModel;
            _manageFilterViewModel = views.ManageFilterViewModel;
        }

        #endregion Constructors

        #region Properties

        public IFilterTranslator _filterTranslator { get; }

        public ManageRepositoryViewModel _manageRepositoryViewModel { get; }

        public bool IsFilterVisible
        {
            get => _isFilterVisible;
            set => Set(ref _isFilterVisible, value, nameof(IsFilterVisible));
        }

        public ObservableCollection<MenuItemModel> MenuFilter
        {
            get => _menuFilter;
            set => Set(ref _menuFilter, value, nameof(MenuFilter));
        }

        public ObservableCollection<MenuItemModel> MenuRepository
        {
            get => _menuFile;
            set => Set(ref _menuFile, value, nameof(MenuRepository));
        }

        #endregion Properties

        #region Methods

        public async Task ActivateLogsAsync(IEnumerable<LogRow> logs)
        {
            var cfg = await _configurationManager.GetAsync();

            _vmLogsViewModel.IsLoggerVisible = cfg.Ui.ShowLogger;
            _vmLogsViewModel.IsThreadIdVisible = cfg.Ui.ShowThreadId;
            _vmLogsViewModel.Logs = new ObservableCollection<LogRow>(logs);
            _vmLogsViewModel.Cache(logs);
            ActivateItem(_vmLogsViewModel);
        }

        public async Task Handle(UiEvent message)
        {
            if (message.Event == UiEvents.RefreshMenus)
            {
                await LoadMenusAsync();
            }
            else if (message.Event == UiEvents.FilterVisibility)
            {
                if (message.Context is bool isVisible) { IsFilterVisible = isVisible; }
            }
        }

        public async Task LoadMenusAsync()
        {
            try
            {
                var app = await _configurationManager.GetAsync();
                var fmanager = await _configurationManager.BuildFilterManagerAsync();

                var menuRepository = LoadMenuRepository(app);
                var menuFilter = LoadMenuFilter(app, fmanager);

                MenuRepository = new ObservableCollection<MenuItemModel>(menuRepository);
                MenuFilter = new ObservableCollection<MenuItemModel>(menuFilter);
            }
            catch (Exception ex) { throw ex; }
        }

        public async void ManageFilters()
        {
            await _manageFilterViewModel.LoadAsync();
            ActivateItem(_manageFilterViewModel);
        }

        //TODO: Error handling
        public async void ManageRepositories()
        {
            await _manageRepositoryViewModel.LoadAsync();
            ActivateItem(_manageRepositoryViewModel);
        }

        private void LoadFilter(IFilterComposite filterComposite)
        {
            _vmLogsViewModel.ResetCache();
            var logs = filterComposite.Filter(_vmLogsViewModel.Logs);
            _vmLogsViewModel.Logs = new ObservableCollection<LogRow>(logs);
        }

        private void LoadLogs(PluginBase plugin)
        {
            var days = plugin.GetDays();
            _vmDaysViewModel.Days = new ObservableCollection<DateTime>(days);
            _vmDaysViewModel.Plugin = plugin;

            _vmLogsViewModel.ClearCache();

            ActivateItem(_vmDaysViewModel);
        }

        private IEnumerable<MenuItemModel> LoadMenuFilter(AppSettings app, IFilterManager fManager)
        {
            var menus = new List<MenuItemModel>();
            foreach (var filter in app.Filters)
            {
                menus.Add(new MenuItemModel
                {
                    Name = filter.Name ?? _filterTranslator.Translate(filter),
                    MenuCommand = new RelayCommand(() => LoadFilter(fManager.Build(filter.Id))),
                });
            }
            return menus;
        }

        private IEnumerable<MenuItemModel> LoadMenuRepository(AppSettings app)
        {
            var pil = _pluginInfoManager.GetPluginsInfo();
            var repositories = (from r in app.Repositories
                                where pil.Where(e => e.Id == r.PluginId).Count() > 0
                                select r);

            var menus = new List<MenuItemModel>();
            foreach (var repo in repositories)
            {
                menus.Add(new MenuItemModel
                {
                    Name = repo.Name,
                    MenuCommand = new RelayCommand(() => LoadLogs(_pluginManager.Build(repo)))
                });
            }
            return menus;
        }

        #endregion Methods
    }
}