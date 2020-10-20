using Caliburn.Micro;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Constants;
using Probel.LogReader.Core.Filters;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Helpers;
using Probel.LogReader.Models;
using Probel.LogReader.Ui;
using Probel.LogReader.ViewModels.Packs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Probel.LogReader.ViewModels
{
    public class MainViewModel : Conductor<IScreen>, IHandle<UiEvent>
    {
        #region Fields

        private readonly IConfigurationManager _configurationManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IFilterTranslator _filterTranslator;
        private readonly ManageFilterBindingsViewModel _manageFilterBindingsViewModel;
        private readonly ManageFilterViewModel _manageFilterViewModel;
        private readonly ManageRepositoryViewModel _manageRepositoryViewModel;
        private readonly IPluginInfoManager _pluginInfoManager;
        private readonly IPluginManager _pluginManager;
        private readonly IUserInteraction _userInteraction;
        private readonly LogsViewModel _vmLogsViewModel;
        private IFilterManager _fmanager;
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
            , IEventAggregator eventAggregator
            , IUserInteraction userInteraction)
        {
            eventAggregator.Subscribe(this);
            _eventAggregator = eventAggregator;

            _configurationManager = cfg;
            _userInteraction = userInteraction;
            _pluginInfoManager = pluginInfoManager;
            _pluginManager = pluginManager;
            _filterTranslator = filterTranslator;
            _vmLogsViewModel = views.LogsViewModel;
            _manageRepositoryViewModel = views.ManageRepositoryViewModel;
            _manageFilterViewModel = views.ManageFilterViewModel;
            _manageFilterBindingsViewModel = views.ManageFilterBindingsViewModel;
        }

        #endregion Constructors

        #region Properties
        protected override void OnActivate()
        {
            LoadRepositoryMenu();
            _vmLogsViewModel.Repositories = MenuRepository;
            ActivateItem(_vmLogsViewModel);
            base.OnActivate();
        }
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

        private void LoadFilter(IFilter filter, string filterName)
        {
            using (_userInteraction.NotifyWait())
            {
                _vmLogsViewModel.LastFilter = filter;
                var logs = filter.Filter(_vmLogsViewModel.GetLogRows());
                _vmLogsViewModel.Cache(logs);
                _vmLogsViewModel.Filter();
                _eventAggregator.PublishOnUIThread(UiEvent.FilterApplied(filterName));
            }
        }

        private IEnumerable<MenuItemModel> LoadMenuFilter(AppSettings app, IFilterManager fManager, Guid repositoryId)
        {
            var menus = new List<MenuItemModel>();
            var aps = _configurationManager.GetDecorated();
            var filters = aps.GetFilters(repositoryId, OrderBy.Asc);
            foreach (var filter in filters)
            {
                var filterName = filter.Name ?? _filterTranslator.Translate(filter);
                menus.Add(new MenuItemModel
                {
                    Name = filterName,
                    MenuCommand = new RelayCommand(() => LoadFilter(fManager.Build(filter.Id), filterName)),
                });
            }
            return menus;
        }

        private IEnumerable<MenuItemModel> LoadMenuRepository(AppSettings app)
        {
            var pil = _pluginInfoManager.GetPluginsInfo();
            var repositories = (from r in app.Repositories
                                where pil.Where(e => e.Id == r.PluginId).Count() > 0
                                select r).OrderBy(e => e.Name);

            var menus = new List<MenuItemModel>();
            foreach (var repo in repositories)
            {
                menus.Add(new MenuItemModel
                {
                    Name = repo.Name,
                    MenuCommand = new RelayCommand(() =>
                    {
                        var menuFilter = LoadMenuFilter(app, _fmanager, repo.Id);
                        MenuFilter = new ObservableCollection<MenuItemModel>(menuFilter);
                        LoadRepository(_pluginManager.Build(repo));
                    })
                });
            }
            return menus;
        }

        public void Handle(UiEvent message)
        {
            if (message.Event == UiEvents.RefreshMenus)
            {
                LoadMenus();
            }
            else if (message.Event == UiEvents.FilterVisibility && message.Context is bool isVisible)
            {
                IsFilterVisible = isVisible;
            }
        }

        public void LoadMenus()
        {
            try
            {
                var t1 = Task.Run(() =>
                {
                    _fmanager = _configurationManager.BuildFilterManager();
                    LoadRepositoryMenu();
                });
                t1.OnErrorHandle(_userInteraction);
            }
            catch (Exception ex) { throw ex; }
        }

        private void LoadRepositoryMenu()
        {
            var app = _configurationManager.Get();
            var menuRepository = LoadMenuRepository(app);
            MenuRepository = new ObservableCollection<MenuItemModel>(menuRepository);
        }

        public void LoadRepository(IPlugin plugin)
        {
            var token = new CancellationToken();
            var scheduler = TaskScheduler.Current;

            var t1 = Task.Run(() =>
            {
                using (_userInteraction.NotifyWait())
                {
                    var cfg = _configurationManager.Get();

                    var orderby = cfg.Ui.IsLogOrderAsc ? OrderBy.Asc : OrderBy.Desc;
                    var days = plugin.GetDays();

                    _vmLogsViewModel.IsOrderByAsc = cfg.Ui.IsLogOrderAsc;
                    _vmLogsViewModel.IsLoggerVisible = cfg.Ui.IsLoggerVisible;
                    _vmLogsViewModel.IsThreadIdVisible = cfg.Ui.IsThreadIdVisible;
                    _vmLogsViewModel.RepositoryName = plugin.RepositoryName;
                    _vmLogsViewModel.Plugin = plugin;
                    _vmLogsViewModel.Listener = plugin;
                    _vmLogsViewModel.Filters = MenuFilter;
                    _vmLogsViewModel.Repositories = MenuRepository;

                    _vmLogsViewModel.LoadDays(days);
                    _eventAggregator.PublishOnUIThread(UiEvent.FilterApplied(string.Empty));

                    _vmLogsViewModel.IsFile = plugin.TryGetFile(out var path);
                    _vmLogsViewModel.CanListen = plugin.CanListen;
                    _vmLogsViewModel.FilePath = path;
                }
            });
            t1.OnErrorHandle(_userInteraction);

            var t2 = t1.ContinueWith(r => ActivateItem(_vmLogsViewModel), token, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
            t2.OnErrorHandle(_userInteraction, token, scheduler);
        }

        public void ManageFilterBindings()
        {
            _manageFilterBindingsViewModel.Load();
            ActivateItem(_manageFilterBindingsViewModel);
        }

        public void ManageFilters()
        {
            _manageFilterViewModel.Load();
            ActivateItem(_manageFilterViewModel);
        }

        public void ManageRepositories()
        {
            _manageRepositoryViewModel.Load();
            ActivateItem(_manageRepositoryViewModel);
        }

        #endregion Methods
    }
}