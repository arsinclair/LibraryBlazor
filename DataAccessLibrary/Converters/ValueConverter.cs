using DataAccessLibrary.Cache;
using DataAccessLibrary.Models;
using DataAccessLibrary.Models.Metadata;
using DataAccessLibrary.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Converters
{
    public class ValueConverter
    {
        public static string ConvertToSQL(object value, string fieldTypeName, bool isLikeClause)
        {
            switch (fieldTypeName)
            {
                case "text": return EscapeForSQL(value.ToString(), isLikeClause);
                case "textarea": return EscapeForSQL(value.ToString(), isLikeClause);
                case "number": return value.ToString();
                case "guid": return EscapeForSQL(value.ToString(), isLikeClause);
                case "boolean": return (bool)value == true ? "1" : "0";
                case "datetime": return EscapeForSQL(((DateTime)value).ToUniversalTime().ToString(), isLikeClause);
                case "entityreference": return EscapeForSQL(((EntityReference)value).Id.ToString(), isLikeClause);
                default: throw new NotImplementedException();
            }
        }

        private static string EscapeForSQL(string unescapedString, bool isLikeClause)
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
