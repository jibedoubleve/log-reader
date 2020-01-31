namespace Probel.LogReader.Core.Filters
{
    public interface IFilterComposite : IFilter
    {
        #region Methods

        void Add(params IFilterExpression[] filters);

        #endregion Methods
    }
}