namespace Probel.LogReader.Core.Filters
{
    public interface IFilterExpression : IFilter
    {
        #region Properties

        string Operand { get; }
        string Operator { get; }
        string Type { get; }

        #endregion Properties
    }
}