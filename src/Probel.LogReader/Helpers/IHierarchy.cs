using System.Collections.Generic;

namespace Probel.LogReader.Helpers
{
    public interface IHierarchy<T>
    {
        #region Properties

        IEnumerable<IHierarchy<T>> Children { get; set; }
        int Level { get; set; }

        T Value { get; set; }

        #endregion Properties
    }
}