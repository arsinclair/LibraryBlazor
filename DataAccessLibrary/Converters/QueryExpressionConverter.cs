﻿using DataAccessLibrary.Cache;
using DataAccessLibrary.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DataAccessLibrary.Converters
{
    public static class QueryExpressionConverter
    {
        private const string ColumnNameRegexPattern = "[^0-9A-Za-z_]";

        public static string ConvertToSQL(QueryExpression query)
        {
            ThrowIfNotValid(query);

            string tableName = CacheManager.GetDatabaseCache().GetTableName(query.EntityName);
            string sql = string.Empty;

            sql += $"SELECT{BuildColumns(query)}";
            sql += $"FROM[{tableName}]";
            sql += BuildWhereClause(query);
            sql += BuildOrderByWithOffset(query);
            sql += ";";

            return sql;
        }

        private static void ThrowIfNotValid(QueryExpression query)
        {
            if (string.IsNullOrEmpty(query.EntityName))
            {
                throw new ArgumentNullException("Entity Name is not set.");
            }
        }

        internal static string BuildColumns(QueryExpression query)
        {
            string output = string.Empty;

            if (query.ColumnSet.AllColumns)
            {
                output = "*";
            }
            else
            {
                var listWithDuplicates = new List<string>() { "[Id]" }; // Id column should be returned with all queries
                foreach (var column in query.ColumnSet.Columns)
                {
                    if (string.IsNullOrEmpty(column)) continue;
                    if (Regex.IsMatch(column, ColumnNameRegexPattern)) throw new ArgumentException($"Query Expression Column Name conatins invalid characters. {column}");

                    listWithDuplicates.Add($"[{column}]");
                }

                output = (string.Join(",", listWithDuplicates.Distinct(StringComparer.OrdinalIgnoreCase)));
            }

            return output;
        }

        internal static string BuildWhereClause(QueryExpression query)
        {
            return FilterExpressionConverter.ConvertToSQL(query.Criteria, query.EntityName);
        }

        internal static string BuildOrderByWithOffset(QueryExpression query)
        {
            List<string> output = new List<string>();

            foreach (OrderExpression sortOrder in query.Orders)
            {
                if (!string.IsNullOrEmpty(sortOrder.AttributeName))
                {
                    if (Regex.IsMatch(sortOrder.AttributeName, ColumnNameRegexPattern))
                        throw new ArgumentException($"Query Expression Sort Order Attribute Name conatins invalid characters. {sortOrder.AttributeName}");
                    if (!string.IsNullOrEmpty(sortOrder.FallbackAttributeName) && Regex.IsMatch(sortOrder.FallbackAttributeName, ColumnNameRegexPattern))
                        throw new ArgumentException($"Query Expression Sort Order Fallback Attribute Name conatins invalid characters. {sortOrder.AttributeName}");

                    string orderType = string.Empty;
                    if (sortOrder.OrderType == OrderType.Ascending)
                    {
                        orderType = "ASC";
                    }
                    else if (sortOrder.OrderType == OrderType.Descending)
                    {
                        orderType = "DESC";
                    }
                    string expression = $"[{sortOrder.AttributeName}]";
                    if (!string.IsNullOrEmpty(sortOrder.FallbackAttributeName))
                        expression = $" COALESCE({expression},[{sortOrder.FallbackAttributeName}])";
                    output.Add($"{expression}{orderType}");
                }
                else if (!string.IsNullOrEmpty(sortOrder.FallbackAttributeName)) {
                    throw new ArgumentException("Can't use a FallbackAttributeName with empty or missing AttributeName in the Order Clause of the Query Expression.");
                }
            }

            if (output.Count == 0 && query.PageInfo.Count > 0)
            {
                throw new ArgumentException("Can't use pagination (PageInfo OFFSET Query) without ORDER BY Clause");
            }

            string offsetClause = BuildOffset(query);
            if (!string.IsNullOrEmpty(offsetClause))
                offsetClause = " " + offsetClause;

            return output.Count > 0
                ? $"ORDER BY{string.Join(",", output)}{offsetClause}"
                : string.Empty;
        }

        internal static string BuildOffset(QueryExpression query)
        {
            string output = string.Empty;

            if (query.PageInfo.Count > 0)
            {
                output = $"OFFSET {query.PageInfo.PageNumber * query.PageInfo.Count} ROWS FETCH NEXT {query.PageInfo.Count} ROWS ONLY";
            }

            return output;
        }
    }
}
