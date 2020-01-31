using Probel.LogReader.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Probel.LogReader.Core.Configuration
{
    public class AppSettingsDecorator
    {
        #region Fields

        private static readonly Guid _idAllLogs = new Guid("01f36d50-5051-4def-a4b4-3665c0b3b00d");
        private readonly AppSettings _appSettings;

        #endregion Fields

        #region Constructors

        public AppSettingsDecorator(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<FilterSettings> GetFilters(OrderBy orderBy = OrderBy.None)
        {
            var filters = GetAllFilters();
            var specialFilter = GetSpecialFilter();

            IEnumerable<FilterSettings> result;
            switch (orderBy)
            {
                case OrderBy.Asc:
                    result = filters.OrderBy(e => e.Name);
                    break;

                case OrderBy.Desc:
                    result = filters.OrderByDescending(e => e.Name);
                    break;

                case OrderBy.None:
                    result = filters;
                    break;

                default:
                    throw new NotSupportedException($"The clause 'OrderBy.{orderBy}' is not supported");
            }

            var r = result.ToList();
            r.Add(specialFilter);
            return r;
        }

        private FilterSettings GetSpecialFilter()
        {
            var result = (from f in _appSettings.Filters
                          where f.Id == _idAllLogs
                          select f).Single();
            return result;
        }

        private IEnumerable<FilterSettings> GetAllFilters()
        {
            var result = (from f in _appSettings.Filters
                          where f.Id != _idAllLogs
                          select f);
            return result;
        }
    }

    #endregion Methods
}
