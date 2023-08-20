using DataAccessLibrary.Query;

namespace DataAccessLibrary.Converters
{
    public static class FilterExpressionConverter
    {
        /// <summary>
        /// Convert a FilterExpression instance to the SQL WHERE clause.
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <param name="depth">For internal use. It facilitates building the WHERE clause. If depth is zero, or not provided, this will append the 'WHERE' word to the output.</param>
        /// <returns></returns>
        public static string ConvertToSQL(FilterExpression filterExpression, string entityName, int depth = 0)
        {
            depth++;
            string output = string.Empty;
            bool hasQueries = filterExpression.Queries != null && filterExpression.Queries.Count > 0;
            bool hasSubQueries = filterExpression.SubQueries != null && filterExpression.SubQueries.Count > 0;

            if (hasQueries || hasSubQueries)
            {
                if (depth == 1) output += "WHERE";

                string operatorSQL = LogicalOperatorConverter.ConvertToSQL(filterExpression.QueryOperator);
                output += "(";

                if (hasQueries)
                {
                    for (int i = 0; i < filterExpression.Queries.Count; i++)
                    {
                        if (string.IsNullOrEmpty(filterExpression.Queries[i].EntityName))
                        {
                            filterExpression.Queries[i].EntityName = entityName; // not tested
                        }
                        output += ConditionExpressionConverter.ConvertToSQL(filterExpression.Queries[i]);
                        var isLastQuery = (i == filterExpression.Queries.Count - 1);
                        if (!isLastQuery || hasSubQueries)
                        {
                            if (char.IsLetterOrDigit(output[output.Length - 1])) output += " ";
                            output += operatorSQL;
                        }
                    }
                }
                if (hasSubQueries)
                {
                    for (int i = 0; i < filterExpression.SubQueries.Count; i++)
                    {
                        output += FilterExpressionConverter.ConvertToSQL(filterExpression.SubQueries[i], entityName, depth);
                        if (i != filterExpression.SubQueries.Count - 1)
                        {
                            if (char.IsLetterOrDigit(output[output.Length - 1])) output += " ";
                            output += operatorSQL;
                        }
                    }
                }
                output += ")";
            }

            return output;
        }
    }
}
