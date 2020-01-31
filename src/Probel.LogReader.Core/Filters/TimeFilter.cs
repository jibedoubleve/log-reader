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
                //r.Time.Date == now.Date &&
                case ">": return (r, t) => r.Time.Date == now.Date && r.Time.TrimSeconds() > now.AddMinutes(Parse(t));
                case ">=": return (r, t) => r.Time.Date == now.Date && r.Time.TrimSeconds() >= now.AddMinutes(Parse(t));
                case "<": return (r, t) => r.Time.Date == now.Date && r.Time.TrimSeconds() < now.AddMinutes(Parse(t));
                case "<=": return (r, t) => r.Time.Date == now.Date && r.Time.TrimSeconds() <= now.AddMinutes(Parse(t));
                default: throw new NotSupportedException($"Cannot build a filter. Operator '{Operator}' is not supported.");
            }
        }

        private double Parse(string time)
        {
            if (int.TryParse(Operand, out var t)) { return t; }
            else { throw new InvalidOperationException($"The time '{time}' cannot be casted as a number"); }
        }

        #endregion Methods
    }
}