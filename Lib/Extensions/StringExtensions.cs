using System;

namespace Lib.Extensions
{
    public static class StringExtensions
    {
        public static string GetValueOrEmpty(this string value) => value ?? string.Empty;

        public static bool IsMatchingPattern(this string pattern, string input)
        {
            return string.IsNullOrWhiteSpace(pattern) ||
                   input?.Contains(pattern, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}
