using System;
using System.Collections.Generic;
using System.Linq;

namespace Probel.LogReader.Core.Filters
{

    public sealed class FilterAttribute : Attribute
    {
        #region Constructors

        public FilterAttribute(string name, FilterOperations operations)
        {
            Operation = operations;
            Name = name.ToLower();
        }

        #endregion Constructors

        #region Properties

        public string Name { get; private set; }
        public FilterOperations Operation { get; set; }

        #endregion Properties
    }
}