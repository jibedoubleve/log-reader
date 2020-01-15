namespace Probel.LogReader.Core.Constants
{
    public static class FilterType
    {
        #region Fields

        public const string Category = "category";
        public const string Level = "level";
        public const string Time = "time";

        #endregion Fields
    }

    public static class TimeFilterOperators
    {
        #region Fields

        public const string Exactly = "==";
        public const string LessThan = "<";
        public const string LessThanOrEqual = "<=";
        public const string MoreThan = ">";
        public const string MoreThanOrEqual = ">=";

        #endregion Fields
    }
}