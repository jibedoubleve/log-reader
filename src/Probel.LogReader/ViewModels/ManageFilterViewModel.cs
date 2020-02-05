﻿using Caliburn.Micro;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Helpers;
using Probel.LogReader.Properties;
using Probel.LogReader.Ui;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Probel.LogReader.ViewModels
{
    public class ManageFilterViewModel : Conductor<IScreen>
    {
        #region Fields

        private readonly IConfigurationManager _configManager;
        private readonly EditFilterViewModel _editFilterViewModel;
        private readonly IEventAggregator _eventAggregator;
        private AppSettings _app;
        private FilterSettings _currentFilterSettings;
        private ObservableCollection<FilterSettings> _filters;

        #endregion Fields

        #region Constructors

        public ManageFilterViewModel(IConfigurationManager configManager
            , EditFilterViewModel editSubfilterViewModel
            , IEventAggregator eventAggregator
            , IUserInteraction userInteraction)
        {
            DeleteCurrentFilterCommand = new RelayCommand(DeleteCurrentFilter);

            _userInteraction = userInteraction;
            _eventAggregator = eventAggregator;
            _editFilterViewModel = editSubfilterViewModel;
            _configManager = configManager;
        }

        #endregion Constructors

        #region Properties

        public FilterSettings CurrentFilter
        {
            get => _currentFilterSettings;
            set => Set(ref _currentFilterSettings, value, nameof(CurrentFilter));
        }

        public ICommand DeleteCurrentFilterCommand { get; private set; }

        private readonly IUserInteraction _userInteraction;

        public ObservableCollection<FilterSettings> Filters
        {
            get => _filters;
            set => Set(ref _filters, value, nameof(Filters));
        }

        #endregion Properties

        #region Methods

        public void ActivateCurrentFilter()
        {
            DeactivateItem(_editFilterViewModel, true);

            if (CurrentFilter != null)
            {
                _editFilterViewModel.SetSubfilters(CurrentFilter);
                NotifyOfPropertyChange(() => CanCreateSubFilter);
                ActivateItem(_editFilterViewModel);
            }
        }

        public void CreateFilter()
        {
            var newFilter = new FilterSettings("new empty");
            Filters.Add(newFilter);
            _app.Filters.Add(newFilter);
            CurrentFilter = newFilter;
        }

        public bool CanCreateSubFilter => _editFilterViewModel.Subfilters != null;
        public void CreateSubFilter() => _editFilterViewModel?.CreateSubfilter();

        public void DiscardAll()
        {
            if (_userInteraction.Ask(Strings.Msg_AskReset) == UserAnswers.Yes)
            {
                Load();
            }
        }

        public void Load()
        {
            _app = Task.Run(() => _configManager.Get()).Result;

            var filters = (from f in _app.Filters
                           where f.Id != FilterSettings.NoFilter.Id
                           select f).ToList();

            Filters = new ObservableCollection<FilterSettings>(filters);
        }

        public void SaveAll()
        {
            Task.Run(() => _configManager.SaveAsync(_app)).Wait();

            _eventAggregator.PublishOnBackgroundThread(UiEvent.RefreshMenus);
            _userInteraction.Inform(Strings.Msg_InformSaved);
        }

        private void DeleteCurrentFilter()
        {
            var toDel = (from f in Filters
                         where f.Id == CurrentFilter.Id
                         select f).FirstOrDefault();

            if (_userInteraction.Ask(Strings.Msg_AskDelete) == UserAnswers.Yes)
            {
                if (toDel != null)
                {
                    Filters.Remove(toDel);
                    _app.Filters.Remove(toDel);
                }
            }
        }

        #endregion Methods
    }
}