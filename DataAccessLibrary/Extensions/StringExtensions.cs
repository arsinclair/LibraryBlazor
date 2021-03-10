using System;

namespace DataAccessLibrary.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string value, StringComparison comparisonMethod)
        {
            return source?.IndexOf(value, comparisonMethod) >= 0;
        }
    }
}
