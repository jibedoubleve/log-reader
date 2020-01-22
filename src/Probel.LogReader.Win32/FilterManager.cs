using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Probel.LogReader.Win32
{
    public class FilterManager : IFilterManager
    {
        #region Fields

        private IList<FilterSettings> _filters = new List<FilterSettings>();

        #endregion Fields

        #region Constructors

        public FilterManager(IList<FilterSettings> filters) => _filters = filters ?? new List<FilterSettings>();

        public FilterManager() => _filters = new List<FilterSettings>();

        #endregion Constructors

        #region Properties

        private IEnumerable<FilterOperations> Operations => Enum.GetValues(typeof(FilterOperations)).Cast<FilterOperations>();

        #endregion Properties

        #region Methods

        public IFilterComposite Build(IEnumerable<FilterExpressionSettings> expression)
        {
            var filters = new List<IFilterExpression>();

            foreach (var item in expression) { filters.Add(Create(item)); }

            var composite = new AndFilterComposite();
            composite.Add(filters.ToArray());

            return composite;
        }

        public IFilterComposite Build(Guid id)
        {
            var expStg = (from f in _filters
                          where f.Id == id
                          select f.Expression).FirstOrDefault();

            if (expStg != null) { return Build(expStg); }
            else { throw new NotSupportedException($"No filter expression with name '{id}' found in the configuration"); }
        }

        public static IEnumerable<string> GetOperators(FilterOperations operation) => FilterHelper.GetOperators(operation);

        private IFilterExpression Create(FilterExpressionSettings item)
        {
            var type = (from t in Assembly.GetAssembly(typeof(IFilter)).GetTypes()
                        where t.GetCustomAttribute<FilterAttribute>() != null
                           && t.IsClass
                           && (t.GetCustomAttribute<FilterAttribute>()?.Name ?? "") == item.Operation.ToLower()
                           && t.GetInterfaces().Any(e => e == typeof(IFilterExpression))
                        select t).FirstOrDefault();

            if (type != null)
            {
                var filter = (BaseFilter)Activator.CreateInstance(type);

                filter.Type = item.Operation;
                filter.Operator = item.Operator;
                filter.Operand = item.Operand;

                return filter;
            }
            else { return new EmptyFilter(); }
        }

        #endregion Methods
    }
}