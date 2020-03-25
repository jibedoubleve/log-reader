using Caliburn.Micro;
using Probel.LogReader.Helpers;
using System;
using System.Collections.Generic;

namespace Probel.LogReader.Models
{
    public class DateHierarchy : PropertyChangedBase, IHierarchy<DateTime>
    {
        #region Properties

        private bool _isExpanded;

        public bool IsExpanded
        {
            get => _isExpanded;
            set => Set(ref _isExpanded, value, nameof(IsExpanded));
        }

        public IList<IHierarchy<DateTime>> Children { get; set; }

        public int Level { get; set; }

        public DateTime Value { get; set; }
    }

    #endregion Properties
}