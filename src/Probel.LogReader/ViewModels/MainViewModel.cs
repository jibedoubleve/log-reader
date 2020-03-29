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
        private readonly IEventAggregator _eventAggregator;
        #region Fields

        private readonly IConfigurationManager _configurationManager;
        private readonly IFilterTranslator _filterTranslator;
        private readonly ManageFilterViewModel _manageFilterViewModel;
        private readonly ManageRepositoryViewModel _manageRepositoryViewModel;
        private readonly IPluginInfoManager _pluginInfoManager;
        private readonly IPluginManager _pluginManager;
        private readonly IUserInteraction _userInteraction;
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
        }

        #endregion Constructors

        #region Properties

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
                    _vmLogsViewModel.IsThreadIdVisible = cfg.Ui.isThreadIdVisible;
                    _vmLogsViewModel.IsDetailVisible = cfg.Ui.IsDetailVisible;
                    _vmLogsViewModel.RepositoryName = plugin.RepositoryName;
                    _vmLogsViewModel.Plugin = plugin;
                    _vmLogsViewModel.Listener = plugin;

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

        public void LoadMenus()
        {
            try
            {
                var t1 = Task.Run(() =>
                {
                    var app = _configurationManager.Get();
                    var fmanager = _configurationManager.BuildFilterManager();

                    var menuRepository = LoadMenuRepository(app);
                    var menuFilter = LoadMenuFilter(app, fmanager);

                    MenuRepository = new ObservableCollection<MenuItemModel>(menuRepository);
                    MenuFilter = new ObservableCollection<MenuItemModel>(menuFilter);
                });
                t1.OnErrorHandle(_userInteraction);
            }
            catch (Exception ex) { throw ex; }
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

        private IEnumerable<MenuItemModel> LoadMenuFilter(AppSettings app, IFilterManager fManager)
        {
            var menus = new List<MenuItemModel>();
            var aps = new AppSettingsDecorator(app);
            var filters = aps.GetFilters(OrderBy.Asc);
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
                    MenuCommand = new RelayCommand(() => LoadRepository(_pluginManager.Build(repo)))
                });
            }
            return menus;
        }
        #endregion Methods
    }
}