using Probel.LogReader.Core.Constants;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Properties;
using System;
using System.Linq;

namespace Probel.LogReader.Core.Filters
{
    public class FilterTranslator : IFilterTranslator
    {
        #region Methods

        public string Translate(FilterSettings stg)
        {
            if (stg == null) { throw new ArgumentException("Filter settings is null.", nameof(stg)); }

            if (string.IsNullOrEmpty(stg.Name))
            {
                if (stg.Expression.Count() > 0)
                {
                    var e = stg.Expression.ElementAt(0);
                    return Translate(e);
                }
                else { return string.Format($"Expression with id: {stg.Id}"); }
            }
            else { return stg.Name; }
        }

        public string Translate(FilterExpressionSettings e)
        {
            var template = GetTemplate(e.Operation);
            var @operator = GetOperator(e.Operator);
            var operand = e.Operand;

            return string.Format(template, @operator, operand);
        }

        private string GetOperator(string @operator)
        {
            switch (@operator?.ToLower() ?? "NULL")
            {
                case "<": return Resource.Filter_LessThan;
                case "<=": return Resource.Filter_LessThanOrEquals;
                case ">": return Resource.Filter_GreaterThan;
                case ">=": return Resource.Filter_GreaterThanOrEquals;
                case "=": return Resource.Filter_Equals;
                case "!=":return Resource.Filter_NotEquals;
                case "in": return Resource.Filter_In;
                case "not in": return Resource.Filter_NotIn;
                default: throw new NotSupportedException($"The operator '{(@operator ?? "NULL")}' is not yet supported in the filters.");
            }
        }

        private string GetTemplate(string type)
        {
            switch (type.ToLower())
            {
                case FilterType.Time: return Resource.Filter_Time;
                case FilterType.Level: return Resource.Filter_Level;
                case FilterType.Category: return Resource.Filter_Category;
                default: throw new NotSupportedException($"The filter of type '{type}' is not yet supported in the filters.");
            }
        }

        #endregion Methods
    }
}