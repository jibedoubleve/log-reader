using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Helpers;
using System;

namespace Probel.LogReader.Core.Filters
{
    [Filter("Time", FilterOperations.Comparition)]
    public class TimeFilter : BaseFilter
    {
        #region Constructors

        public TimeFilter() : base(FilterOperations.Comparition)
        {
        }

        #endregion Constructors

        #region Methods

        protected override Func<LogRow, string, bool> GetFilter()
        {
            var now = DateTime.Now.TrimSeconds();          
            switch (Operator)
            {
                case ">": return (r, t) => MinutesBetween(r.Time, now) > Parse(t);
                case ">=": return (r, t) => MinutesBetween(r.Time, now) >= Parse(t);
                case "<": return (r, t) => MinutesBetween(r.Time, now) < Parse(t);
                case "<=": return (r, t) => MinutesBetween(r.Time, now) <= Parse(t);
                case "!=": return (r, t) => MinutesBetween(r.Time, now) != Parse(t);
                case "==": return (r, t) => MinutesBetween(r.Time, now) == Parse(t);
                default: throw new NotSupportedException($"Cannot build a filter. Operator '{Operator}' is not supported.");
            }
        }

        private double MinutesBetween(DateTime r, DateTime n) => (int)Math.Abs((r.TrimSeconds() - n.TrimSeconds()).TotalMinutes);

        private double Parse(string time)
        {
            if (int.TryParse(Operand, out var t)) { return t; }
            else { throw new InvalidOperationException($"The time '{time}' cannot be casted as a number"); }
        }

        #endregion Methods
    }
}