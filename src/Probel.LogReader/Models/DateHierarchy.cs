using Probel.LogReader.Helpers;
using System;
using System.Collections.Generic;

namespace Probel.LogReader.Models
{
    public class DateHierarchy : IHierarchy<DateTime>
    {
        #region Properties

        public IEnumerable<IHierarchy<DateTime>> Children { get; set; }
        public int Level { get; set; }

        public DateTime Value { get; set; }
    }

    #endregion Properties
}