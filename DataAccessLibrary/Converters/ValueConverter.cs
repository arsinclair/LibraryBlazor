using DataAccessLibrary.Models;
using System;

namespace DataAccessLibrary.Converters
{
    public class ValueConverter
    {
        public static string ConvertToSQL(object value, string fieldTypeName, bool isLikeClause)
        {
            switch (fieldTypeName)
            {
                case "Text": return EscapeForSQL(value.ToString(), isLikeClause);
                case "TextArea": return EscapeForSQL(value.ToString(), isLikeClause);
                case "Number": return value.ToString();
                case "Guid": return EscapeForSQL(value.ToString(), isLikeClause);
                case "Boolean": return (bool)value == true ? "1" : "0";
                case "DateTime": return EscapeForSQL(((DateTime)value).ToUniversalTime().ToString(), false);
                case "EntityReference": return EscapeForSQL(((EntityReference)value).Id.ToString(), isLikeClause);
                default: throw new NotImplementedException();
            }
        }

        public static string EscapeForSQL(string unescapedString, bool isLikeClause)
        {
            string output = unescapedString;
            if (isLikeClause)
            {
                output = EscapeForLike(output);
                output = $"%{output}%";
            }
            output = output.Replace("'", "''");
            output = $"'{output}'";
            return output;
        }

        private static string EscapeForLike(string unescapedString)
        {
            return unescapedString
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("_", "[_]");
        }
    }
}
