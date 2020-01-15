using Caliburn.Micro;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Filters;
using Probel.LogReader.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace Probel.LogReader.ViewModels
{
    public class EditFilterViewModel : Conductor<IScreen>
    {
        #region Fields

        private readonly EditSubfilterViewModel _editSubfilterViewModel;
        private FilterExpressionSettings _currentSubfilter;
        private ObservableCollection<FilterExpressionSettings> _subfilters;

        #endregion Fields

        #region Constructors

        public EditFilterViewModel(IFilterTranslator filterTranslator, EditSubfilterViewModel editSubfilterViewModel)
        {
            DeleteCurrentFilterCommand = new RelayCommand(DeleteCurrentFilter);

            _editSubfilterViewModel = editSubfilterViewModel;
            FilterTranslator = filterTranslator;
            _subfilters = new ObservableCollection<FilterExpressionSettings>();
        }

        #endregion Constructors

        #region Properties

        public IList<FilterExpressionSettings> _cachedSubfilter { get; private set; }

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

        public IFilterTranslator FilterTranslator { get; }

        public ObservableCollection<FilterExpressionSettings> Subfilters
        {
            get => _subfilters;
            private set => Set(ref _subfilters, value, nameof(Subfilters));
        }

        #endregion Properties

        #region Methods

        public void CreateSubfilter()
        {
            var newFilter = new FilterExpressionSettings() { Operand = "15", Operator = "<=", Operation = "time" };

            Subfilters.Add(newFilter);
            _cachedSubfilter.Add(newFilter);

            CurrentSubfilter = newFilter;
        }

        public void ActivateCurrentSubfilter()
        {
            _editSubfilterViewModel.Load(CurrentSubfilter);
            ActivateItem(_editSubfilterViewModel);
        }

        public void Reset()
        {
            CurrentSubfilter = null;
            DeactivateItem(_editSubfilterViewModel, true);
        }

        public void SetSubfilters(IList<FilterExpressionSettings> subfilters)
        {
            _cachedSubfilter = subfilters;
            Subfilters = new ObservableCollection<FilterExpressionSettings>(subfilters);
        }


        public ICommand DeleteCurrentFilterCommand { get; private set; }
        public void DeleteCurrentFilter()
        {
            _cachedSubfilter.Remove(CurrentSubfilter);
            Subfilters.Remove(CurrentSubfilter);
        }
        #endregion Methods
    }
}