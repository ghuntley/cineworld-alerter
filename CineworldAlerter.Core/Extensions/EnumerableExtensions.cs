using System.Collections.Generic;
using System.Linq;

namespace CineworldAlerter.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
            => list == null || !list.Any();
    }
}
