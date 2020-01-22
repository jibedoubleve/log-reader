using Caliburn.Micro;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Helpers;
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
        private readonly EditFilterViewModel _editSubfilterViewModel;
        private readonly IEventAggregator _eventAggregator;
        private AppSettings _app;
        private FilterSettings _currentFilterSettings;
        private ObservableCollection<FilterSettings> _filters;

        #endregion Fields

        #region Constructors

        public ManageFilterViewModel(IConfigurationManager configManager, EditFilterViewModel editSubfilterViewModel, IEventAggregator eventAggregator)
        {
            DeleteCurrentFilterCommand = new RelayCommand(DeleteCurrentFilter);

            _eventAggregator = eventAggregator;
            _editSubfilterViewModel = editSubfilterViewModel;
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

        public ObservableCollection<FilterSettings> Filters
        {
            get => _filters;
            set => Set(ref _filters, value, nameof(Filters));
        }

        #endregion Properties

        #region Methods

        public void ActivateCurrentFilter()
        {
            DeactivateItem(_editSubfilterViewModel, true);

            if (CurrentFilter != null)
            {
                _editSubfilterViewModel.SetSubfilters(CurrentFilter.Expression);
                ActivateItem(_editSubfilterViewModel);
            }
        }

        public void CreateFilter()
        {
            var newFilter = new FilterSettings("new empty");
            Filters.Add(newFilter);
            _app.Filters.Add(newFilter);
            CurrentFilter = newFilter;
        }

        //TODO: Error handling
        public async void DiscardAll() => await LoadAsync();

        public async Task LoadAsync()
        {
            _app = await _configManager.GetAsync();

            var filters = (from f in _app.Filters
                           where f.Id != FilterSettings.NoFilter.Id
                           select f).ToList();

            Filters = new ObservableCollection<FilterSettings>(filters);
        }

        //TODO: Error handling
        public async void SaveAll()
        {
            await _configManager.SaveAsync(_app);
            _eventAggregator.PublishOnBackgroundThread(UiEvent.RefreshMenus);
        }

        private void DeleteCurrentFilter()
        {
            var toDel = (from f in Filters
                         where f.Id == CurrentFilter.Id
                         select f).FirstOrDefault();

            if (toDel != null)
            {
                Filters.Remove(toDel);
                _app.Filters.Remove(toDel);
            }
        }

        #endregion Methods
    }
}