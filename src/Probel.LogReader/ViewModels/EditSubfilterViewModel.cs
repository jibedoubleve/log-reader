using Caliburn.Micro;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Filters;
using System.Collections.ObjectModel;
using System.Linq;

namespace Probel.LogReader.ViewModels
{
    public class EditSubfilterViewModel : Screen
    {
        #region Fields

        private string _currentOperation;
        private string _currentOperator;
        private FilterExpressionSettings _currentSubfilter;
        private string _operand;
        private ObservableCollection<string> _operation;
        private ObservableCollection<string> _operators;

        #endregion Fields

        #region Properties

        public string CurrentOperation
        {
            get => _currentOperation;
            set
            {
                if (Set(ref _currentOperation, value, nameof(CurrentOperation)))
                {
                    if (CurrentSubfilter != null) { CurrentSubfilter.Operation = value; }
                    RefreshOperators(value);
                }
            }
        }

        public string CurrentOperator
        {
            get => _currentOperator;
            set
            {
                if (Set(ref _currentOperator, value, nameof(CurrentOperator)))
                {
                    if (CurrentSubfilter != null && string.IsNullOrEmpty(value) == false) { CurrentSubfilter.Operator = value; }
                }
            }
        }

        public FilterExpressionSettings CurrentSubfilter
        {
            get => _currentSubfilter;
            private set => Set(ref _currentSubfilter, value, nameof(CurrentSubfilter));
        }

        public string Operand
        {
            get => _operand;
            set
            {
                if (Set(ref _operand, value, nameof(Operand)))
                {
                    if (CurrentSubfilter != null) { CurrentSubfilter.Operand = value; }
                }
            }
        }

        public ObservableCollection<string> Operations
        {
            get => _operation;
            set => Set(ref _operation, value, nameof(Operations));
        }

        public ObservableCollection<string> Operators
        {
            get => _operators;
            set => Set(ref _operators, value, nameof(Operators));
        }

        #endregion Properties

        #region Methods

        public void Load(FilterExpressionSettings subfilter)
        {
            CurrentSubfilter = subfilter;

            if (subfilter == null) { return; }

            //---
            Operations = new ObservableCollection<string>(FilterHelper.GetOperations());
            var operation = (from t in Operations
                             where t.ToLower() == CurrentSubfilter.Operation.ToLower()
                             select t).FirstOrDefault();
            CurrentOperation = operation;

            //---
            Operators = new ObservableCollection<string>(FilterHelper.GetOperators(operation));
            var @operator = (from t in Operators
                             where t.ToLower() == (CurrentSubfilter?.Operator ?? "").ToLower()
                             select t).FirstOrDefault();
            CurrentOperator = @operator;

            //---
            Operand = CurrentSubfilter.Operand;
        }

        private void RefreshOperators(string value) => Operators = new ObservableCollection<string>(FilterHelper.GetOperators(value));

        #endregion Methods
    }
}