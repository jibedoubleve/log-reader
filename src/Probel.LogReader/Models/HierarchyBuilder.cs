using Probel.LogReader.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Probel.LogReader.Models
{
    public static class HierarchyBuilder
    {
        #region Methods

        public static IEnumerable<IHierarchy<DateTime>> ToHierarchy(this IEnumerable<DateTime> dates)
        {
            var d = (from date in dates
                     group date by date.Year into year
                     select new DateHierarchy
                     {
                         Level = 1,
                         Value = new DateTime(year.Key, 1, 1),
                         Children = (from date in year
                                     group date by date.Month into month
                                     select new DateHierarchy
                                     {
                                         Level = 2,
                                         Value = new DateTime(year.Key, month.Key, 1),
                                         Children = (from date in month
                                                     select new DateHierarchy
                                                     {
                                                         Level = 3,
                                                         Value = date,
                                                         Children = null,
                                                     }).Cast<IHierarchy<DateTime>>().ToList(),
                                     }).Cast<IHierarchy<DateTime>>().ToList()
                     });
            return d;
        }

        #endregion Methods
    }
}