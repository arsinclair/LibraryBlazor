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
                case "Number": return EscapeForSQL(value.ToString(), isLikeClause);
                case "Guid": return EscapeForSQL(value.ToString(), isLikeClause);
                case "Boolean": return ParseBool(value);
                case "DateTime": return EscapeForSQL(ParseDateTime(value), false);
                case "EntityReference": return EscapeForSQL(ParseEntityReference(value), isLikeClause);
                default: throw new NotImplementedException();
            }
        }

        internal static string EscapeForSQL(string unescapedString, bool isLikeClause)
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

        private static string ParseDateTime(object value)
        {
            if (value == null)
                throw new ArgumentNullException($"Unable to parse a DateTime that is null.");

            if (value is DateTime)
            {
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
            }

            throw new FormatException($"Unable to cast to a DateTime - {value}.");
        }

        private static string ParseBool(object value)
        {
            if (value == null)
                throw new ArgumentNullException($"Unable to parse a Bool that is null.");

            if (value is bool)
            {
                return (bool)value == true ? "1" : "0";
            }

            throw new FormatException($"Unable to cast to a Bool - {value}.");
        }

        private static string ParseEntityReference(object value)
        {
            if (value == null)
                throw new ArgumentNullException($"Unable to parse a Bool that is null.");

            if (value is EntityReference)
            {
                return ((EntityReference)value).Id.ToString();
            }
            else if (value is Guid)
            {
                return value.ToString();
            }
            else
            {
                Guid parsedGuid;
                if (Guid.TryParse(value.ToString(), out parsedGuid))
                {
                    return parsedGuid.ToString();
                }
            }

            throw new FormatException($"Unable to parse an EntityReference - {value}.");
        }
    }
}
