using Probel.LogReader.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Probel.LogReader.Core.Configuration
{
    internal class AppSettingsDecorator : IAppSettingsDecorator
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

        public IEnumerable<FilterSettings> GetFilters(Guid repositoryId, OrderBy orderBy = OrderBy.None)
        {
            var filters = GetAllFilters(repositoryId);
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

        public IEnumerable<FilterSettings> GetFilters(IEnumerable<FilterSettings> exept)
        {
            var result = (from f in GetAllFilters()
                          where exept.Where(e => e.Id == f.Id).Any() == false
                          select f);
            return result;
        }

        private IEnumerable<FilterSettings> GetAllFilters()
        {
            var result = (from f in _appSettings.Filters
                          where f.Id != _idAllLogs
                          select f);
            return result;
        }

        private IEnumerable<FilterSettings> GetAllFilters(Guid repositoryId)
        {
            var keepThem = (from f in _appSettings.RepositoryFilters
                            where f.RepositoryId == repositoryId
                            select f.FilterId);

            var result = (from f in _appSettings.Filters
                          where f.Id != _idAllLogs
                             && keepThem.Contains(f.Id)
                          select f);
            return result;
        }

        public IEnumerable<FilterSettings> GetActiveFilters(Guid repositoryId)
        {
            var ids = (from f in _appSettings.RepositoryFilters
                       where f.RepositoryId == repositoryId
                       select f.FilterId);

            var filters = (from f in _appSettings.Filters
                           where ids.Contains(f.Id)
                           select f).ToList();
            return filters;
        }

        public void Remove(FilterSettings toDel)
        {
            toDel = _appSettings.Filters.Where(e => e.Id == toDel.Id).Single();

            _appSettings.Filters.Remove(toDel);

            var bindings = (from b in _appSettings.RepositoryFilters
                            where b.FilterId == toDel.Id
                            select b).ToList();

            foreach (var binding in bindings) { _appSettings.RepositoryFilters.Remove(binding); }
        }

        public void Remove(RepositorySettings toDel)
        {
            toDel = _appSettings.Repositories.Where(e => e.Id == toDel.Id).Single();

            _appSettings.Repositories.Remove(toDel);

            var bindings = (from b in _appSettings.RepositoryFilters
                            where b.RepositoryId == toDel.Id
                            select b).ToList();

            foreach (var binding in bindings) { _appSettings.RepositoryFilters.Remove(binding); }
        }

        public void BindFilters(Guid repositoryId, IEnumerable<FilterSettings> filters)
        {
            //It works fast enough, I don't need to optimise this code

            var toRemove = (from f in _appSettings.RepositoryFilters
                            where f.RepositoryId == repositoryId
                            select f).ToList();
            foreach (var item in toRemove)
            {
                _appSettings.RepositoryFilters.Remove(item);
            }

            foreach (var filter in filters)
            {
                _appSettings.RepositoryFilters.Add(new RepositoryFilterSettings() { FilterId = filter.Id, RepositoryId = repositoryId });
            }
        }

        public static implicit operator AppSettings(AppSettingsDecorator src) => src._appSettings;

        public AppSettings Cast() => _appSettings;
    }

    #endregion Methods
}