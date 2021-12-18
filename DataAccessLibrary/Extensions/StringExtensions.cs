using System;

namespace DataAccessLibrary.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string value, StringComparison comparisonMethod)
        {
            return source?.IndexOf(value, comparisonMethod) >= 0;
        }

        public static string? Truncate(this string? value, int maxLength, string truncationSuffix = "…")
        {
            return value?.Length > maxLength
                ? value.Substring(0, maxLength) + truncationSuffix
                : value;
        }
    }
}
