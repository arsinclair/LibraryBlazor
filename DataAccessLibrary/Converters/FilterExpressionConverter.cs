using DataAccessLibrary.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Converters
{
    public static class FilterExpressionConverter
    {
        /// <summary>
        /// Convert a FilterExpression instance to the SQL string.
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <param name="depth">For internal use. It facilitates building the WHERE clause. If depth is zero, this will append the 'WHERE' word to the output.</param>
        /// <returns></returns>
        public static string ConvertToSQL(FilterExpression filterExpression, int depth = 0)
        {
            depth++;
            StringBuilder sb = new StringBuilder();
            bool hasQueries = filterExpression.Queries != null && filterExpression.Queries.Count > 0;
            bool hasSubQueries = filterExpression.SubQueries != null && filterExpression.SubQueries.Count > 0;

            if (hasQueries || hasSubQueries)
            {
                if (depth == 1) sb.Append(" WHERE ");

                string operatorSQL = LogicalOperatorConverter.ConvertToSQL(filterExpression.QueryOperator) + " ";
                sb.Append(" ( ");

                if (hasQueries)
                {
                    for (int i = 0; i < filterExpression.Queries.Count; i++)
                    {
                        sb.Append(ConditionExpressionConverter.ConvertToSQL(filterExpression.Queries[i]));
                        var isLastQuery = (i == filterExpression.Queries.Count - 1);
                        if (!isLastQuery || hasSubQueries)
                        {
                            sb.Append(operatorSQL);
                        }
                    }
                }
                if (hasSubQueries)
                {
                    for (int i = 0; i < filterExpression.SubQueries.Count; i++)
                    {
                        sb.Append(FilterExpressionConverter.ConvertToSQL(filterExpression.SubQueries[i], depth));
                        if (i != filterExpression.SubQueries.Count - 1)
                        {
                            sb.Append(operatorSQL);
                        }
                    }
                }
                sb.Append(" ) ");
            }

            return sb.ToString();
        }
    }
}
