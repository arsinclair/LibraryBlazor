using DataAccessLibrary.Cache;
using DataAccessLibrary.Models.Metadata;
using DataAccessLibrary.Query;
using System;
using System.Linq;

namespace DataAccessLibrary.Converters
{
    public static class ConditionExpressionConverter
    {
        public static string ConvertToSQL(ConditionExpression condition)
        {
            if (string.IsNullOrEmpty(condition.AttributeName))
                throw new ArgumentNullException(nameof(condition.AttributeName));

            string output = string.Empty;
            string operatorStr = GetOperatorAsSQL(condition.Operator);

            SysField fieldDefinition = CacheManager.GetDatabaseCache().GetFieldByName(condition.AttributeName, condition.EntityName);
            if (condition.Operator == ConditionOperator.Null || condition.Operator == ConditionOperator.NotNull)
            {
                if (condition.Values != null || condition.Values.Count > 0)
                    throw new Exception($"No values should be provided when using ${condition.Operator} operator.");
                output += $"[{condition.AttributeName}]{operatorStr}";
            }
            else if (condition.Operator == ConditionOperator.In || condition.Operator == ConditionOperator.NotIn)
            {
                if (condition.Values == null || condition.Values.Count == 0)
                    throw new Exception($"At least one value should be provided when using ${condition.Operator} operator.");

                output += $"[{condition.AttributeName}]{operatorStr}(";

                for (int i = 0; i < condition.Values.Count; i++)
                {
                    output += ValueConverter.ConvertToSQL(condition.Values[i], fieldDefinition.Type.Name, false);
                    if (i != condition.Values.Count - 1)
                    {
                        output += ",";
                    }
                }

                output += ")";
            }
            else if (condition.Operator == ConditionOperator.Like || condition.Operator == ConditionOperator.NotLike)
            {
                if (condition.Values == null || condition.Values.Count != 1 || condition.Values[0] == null)
                    throw new Exception($"One and only one value should be provided when using {condition.Operator} operator.");
                if (Array.IndexOf(new[] { "text", "textarea", "number" }, fieldDefinition.Type.Name.ToLower()) == -1)
                    throw new Exception($"{condition.Operator} can only be used with Text, TextArea and Number types.");

                output += $"[{condition.AttributeName}]{operatorStr}";
                output += ValueConverter.ConvertToSQL(condition.Values[0], fieldDefinition.Type.Name, true);
            }
            else
            {
                if (condition.Values == null || condition.Values[0] == null || condition.Values.Count == 0)
                    throw new Exception($"A value should be provided when using {condition.Operator} operator. For Null-checks use {ConditionOperator.Null} operator.");
                if (condition.Values.Count > 1)
                    throw new Exception($"Only one value should be provided when using {condition.Operator} operator.");

                output += $"[{condition.AttributeName}]{operatorStr}";
                output += ValueConverter.ConvertToSQL(condition.Values[0], fieldDefinition.Type.Name, false);
            }

            return output;
        }

        private static string GetOperatorAsSQL(ConditionOperator @operator)
        {
            switch (@operator)
            {
                case ConditionOperator.Equal: return "=";
                case ConditionOperator.NotEqual: return "<>";
                case ConditionOperator.GreaterThan: return ">";
                case ConditionOperator.LessThan: return "<";
                case ConditionOperator.GreaterEqual: return ">=";
                case ConditionOperator.LessEqual: return "<=";
                case ConditionOperator.Like: return "LIKE";
                case ConditionOperator.NotLike: return "NOT LIKE";
                case ConditionOperator.NotNull: return "IS NOT NULL";
                case ConditionOperator.Null: return "IS NULL";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
