using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Probel.LogReader.Core.Filters
{
    public enum FilterOperations
    {
        /// <summary>
        /// This kind of filter is meant to compare with each other when it has the same type
        /// In a word, operations are i.e. greater than, lowe than, equal, ...
        /// </summary>
        Comparition,

        /// <summary>
        /// This kind of filter include or exclude result when item 'is in' or 'not in'
        /// a specified list of items
        /// </summary>
        List,
    }

    public static class FilterHelper
    {
        #region Methods
        public static IEnumerable<string> GetOperations() => Filters.Select(t => t.Name);

        private static IEnumerable<FilterAttribute> Filters
        {
            get
            {
                var result = (from t in Assembly.GetAssembly(typeof(IFilter)).GetTypes()
                              where t.GetCustomAttribute<FilterAttribute>() != null
                                 && t.IsClass
                                 && t.GetInterfaces().Any(e => e == typeof(IFilterExpression))
                              select t.GetCustomAttribute<FilterAttribute>());
                return result;
            }
        }
        public static List<string> GetOperationTypes() => Enum.GetNames(typeof(FilterOperations)).ToList();

        public static IEnumerable<string> GetOperators(string op)
        {
            var fi = (from f in Filters
                     where f.Name.ToLower() == op.ToLower()
                     select f.Operation).FirstOrDefault();

            return GetOperators(fi);
        }

        public static IEnumerable<string> GetOperators(FilterOperations op)
        {
            switch (op)
            {
                case FilterOperations.Comparition: return new string[] { "<", "<=", "=", "!=", ">", ">=" };
                case FilterOperations.List: return new string[] { "in", "not in" };
                default: throw new NotSupportedException($"The operation '{op.ToString()}' is not supported and does not have any operators.");
            }
        }

        #endregion Methods
    }
}