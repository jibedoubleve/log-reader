using System;
using System.Text;

namespace Probel.LogReader.Core.Helpers
{
    public static class ExceptionExtension
    {
        #region Methods

        public static string ToText(this Exception ex)
        {
            if (ex is AggregateException ae)
            {
                var sb = new StringBuilder();
                var tab = "";
                foreach (var e in ae.InnerExceptions)
                {
                    sb.AppendLine(tab + e.Message);
                    tab += "\t";
                }
                return sb.ToString();
            }
            else { return ex.Message; }
        }

        #endregion Methods
    }
}