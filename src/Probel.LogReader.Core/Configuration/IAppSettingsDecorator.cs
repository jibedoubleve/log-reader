using Probel.LogReader.Core.Constants;
using System;
using System.Collections.Generic;

namespace Probel.LogReader.Core.Configuration
{
    public interface IAppSettingsDecorator
    {
        #region Methods

        void BindFilters(Guid repositoryId, IEnumerable<FilterSettings> filters);

        AppSettings Cast();

        IEnumerable<FilterSettings> GetActiveFilters(Guid repositoryId);

        IEnumerable<FilterSettings> GetFilters(Guid repositoryId, OrderBy orderBy = OrderBy.None);

        IEnumerable<FilterSettings> GetFilters(IEnumerable<FilterSettings> exept);

        void Remove(FilterSettings toDel);

        void Remove(RepositorySettings toDel);

        #endregion Methods
    }
}