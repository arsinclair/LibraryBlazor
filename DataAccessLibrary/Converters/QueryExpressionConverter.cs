using DataAccessLibrary.Cache;
using DataAccessLibrary.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLibrary.Converters
{
    public static class QueryExpressionConverter
    {
        public static string ConvertToSQL(QueryExpression query)
        {
            ThrowIfNotValid(query);

            string tableName = CacheManager.GetDatabaseCache().GetTableName(query.EntityName);

            string columnsString = BuildColumns(query);
            string rowCount = BuildOffset(query);
            string orderBy = BuildOrderBy(query);
            string whereString = BuildWhereClause(query);

            string sql = @$"SELECT {columnsString} 
                            FROM {tableName} 
                            {whereString}
                            ORDER BY {orderBy}
                            {rowCount};";

            return sql;
        }

        private static void ThrowIfNotValid(QueryExpression query)
        {
            if (string.IsNullOrEmpty(query.EntityName))
            {
                throw new ArgumentNullException("Entity Name is not set.");
            }
        }


        private static string BuildColumns(QueryExpression query)
        {
            string output;

            if (query.ColumnSet.AllColumns)
            {
                output = "*";
            }
            else
            {
                query.ColumnSet.AddColumn("Id");
                output = string.Join(", ", query.ColumnSet.Columns.ToArray().Distinct(StringComparer.OrdinalIgnoreCase));
            }

            return output;
        }

        private static string BuildWhereClause(QueryExpression query)
        {
            return FilterExpressionConverter.ConvertToSQL(query.Criteria, query.EntityName);
        }

        private static string BuildOrderBy(QueryExpression query)
        {
            string output = string.Empty;

            if (query.Orders.Count > 0)
            {
                for (int i = 0; i < query.Orders.Count; i++)
                {
                    output += query.Orders[i].AttributeName;
                    if (query.Orders[i].OrderType == OrderType.Ascending)
                    {
                        output += " ASC";
                    }
                    else if (query.Orders[i].OrderType == OrderType.Descending)
                    {
                        output += " DESC";
                    }
                    if (i != query.Orders.Count - 1)
                    {
                        output += ", ";
                    }
                }
            }
            else
            {
                output = "Id";
            }

            return output;
        }

        private static string BuildOffset(QueryExpression query)
        {
            string output = string.Empty;

            if (query.PageInfo.Count > 0)
            {
                output = $"OFFSET {query.PageInfo.PageNumber} ROWS FETCH NEXT {query.PageInfo.Count} ROWS ONLY";
            }

            return output;
        }
    }
}
