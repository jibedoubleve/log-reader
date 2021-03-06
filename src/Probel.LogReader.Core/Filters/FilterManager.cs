﻿using Probel.LogReader.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Probel.LogReader.Core.Filters
{
    public class FilterManager : IFilterManager
    {
        #region Fields

        private readonly IList<FilterSettings> _filters = new List<FilterSettings>();

        #endregion Fields

        #region Constructors

        public FilterManager(IList<FilterSettings> filters)
        {
            _filters = filters ?? new List<FilterSettings>();
        }

        public FilterManager()
        {
            _filters = new List<FilterSettings>();
        }

        #endregion Constructors

        #region Methods

        public static IEnumerable<string> GetOperators(FilterOperations operation) => FilterHelper.GetOperators(operation);

        public IFilterComposite Build(IEnumerable<FilterExpressionSettings> expression, string @operator)
        {
            var filters = new List<IFilterExpression>();

            foreach (var item in expression) { filters.Add(Create(item)); }

            var composite = (@operator.ToLower() == "and") ? (IFilterComposite)new AndFilterComposite() : new OrFilterComposite();
            composite.Add(filters.ToArray());

            return composite;
        }

        public IFilterComposite Build(Guid id)
        {
            var expStg = (from f in _filters
                          where f.Id == id
                          select f).FirstOrDefault();

            if (expStg != null) { return Build(expStg.Expression, expStg.Operator); }
            else { throw new NotSupportedException($"No filter expression with name '{id}' found in the configuration"); }
        }

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