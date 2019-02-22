using System;

namespace Cineworld.Api.Extensions
{
    public static class CinemaExtensions
    {
        public static string CleanName(this string name)
        {
            name = name.Replace("-", string.Empty);
            name = name.Replace("  ", " ");
            name = name.Replace(" ", "-");
            name = name.ToLower();

            return name;
        }
    }

    internal static class DateExtensions
    {
        internal static string ToCineworldDate(this DateTimeOffset date)
            => date.Date.ToCineworldDate();

        internal static string ToCineworldDate(this DateTime date)
            => date.ToString("yyyy-MM-dd");
    }
}
