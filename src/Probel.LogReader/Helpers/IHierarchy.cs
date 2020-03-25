using System.Collections.Generic;

namespace Probel.LogReader.Helpers
{
    public interface IHierarchy<T>
    {
        #region Properties

        IList<IHierarchy<T>> Children { get; set; }
        bool IsExpanded { get; set; }
        int Level { get; set; }

        T Value { get; set; }

        #endregion Properties
    }
}