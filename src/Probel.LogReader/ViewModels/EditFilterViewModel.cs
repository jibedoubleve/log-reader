using Caliburn.Micro;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Filters;
using Probel.LogReader.Helpers;
using Probel.LogReader.Properties;
using Probel.LogReader.Ui;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Probel.LogReader.ViewModels
{
    public class EditFilterViewModel : Conductor<IScreen>
    {
        #region Fields

        private readonly EditSubfilterViewModel _editSubfilterViewModel;
        private IList<FilterExpressionSettings> _cachedSubfilter;
        private FilterExpressionSettings _currentSubfilter;
        private FilterSettings _filter;
        private ObservableCollection<FilterExpressionSettings> _subfilters;

        #endregion Fields

        #region Constructors

        public EditFilterViewModel(IFilterTranslator filterTranslator
            , EditSubfilterViewModel editSubfilterViewModel
            , IUserInteraction userInteraction)
        {
            DeleteCurrentFilterCommand = new RelayCommand(DeleteCurrentFilter);

            _userInteraction = userInteraction;
            _editSubfilterViewModel = editSubfilterViewModel;
            FilterTranslator = filterTranslator;
            _subfilters = new ObservableCollection<FilterExpressionSettings>();
        }

        #endregion Constructors

        #region Properties

        public FilterExpressionSettings CurrentSubfilter
        {
            get => _currentSubfilter;
            set
            {
                if (Set(ref _currentSubfilter, value, nameof(CurrentSubfilter)))
                {
                    NotifyOfPropertyChange(nameof(FilterTranslator));
                }
            }
        }

        public ICommand DeleteCurrentFilterCommand { get; private set; }

        private readonly IUserInteraction _userInteraction;

        public FilterSettings Filter
        {
            get => _filter;
            set => Set(ref _filter, value, nameof(Filter));
        }

        public IFilterTranslator FilterTranslator { get; }

        public ObservableCollection<FilterExpressionSettings> Subfilters
        {
            get => _subfilters;
            private set => Set(ref _subfilters, value, nameof(Subfilters));
        }

        #endregion Properties

        #region Methods

        public void ActivateCurrentSubfilter()
        {
            _editSubfilterViewModel.Load(CurrentSubfilter);
            ActivateItem(_editSubfilterViewModel);
        }

        public void CreateSubfilter()
        {
            var newFilter = new FilterExpressionSettings() { Operand = "15", Operator = "<=", Operation = "time" };

            Subfilters.Add(newFilter);
            _cachedSubfilter.Add(newFilter);

            CurrentSubfilter = newFilter;
        }

        public void DeleteCurrentFilter()
        {
            if (_userInteraction.Ask(Strings.Msg_AskDelete) == UserAnswers.Yes)
            {
                _cachedSubfilter.Remove(CurrentSubfilter);
                Subfilters.Remove(CurrentSubfilter);
            }
        }

        public void Reset()
        {
            CurrentSubfilter = null;
            DeactivateItem(_editSubfilterViewModel, true);
        }

        public void SetSubfilters(FilterSettings filter)
        {
            _cachedSubfilter = filter.Expression;
            Subfilters = new ObservableCollection<FilterExpressionSettings>(filter.Expression);
            Filter = filter;
        }

        protected override void OnDeactivate(bool close)
        {
            CurrentSubfilter = null;
            DeactivateItem(_editSubfilterViewModel, close);
        }

        #endregion Methods
    }
}