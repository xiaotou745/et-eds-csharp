using System.Collections.Generic;

namespace ETS.Expand
{
    public class EnumerableExpand
    {
        public static IList<T> ToIList<T>(IEnumerable<T> enumerable)
        {
            return enumerable is IList<T> ? (IList<T>) enumerable : new List<T>(enumerable);
        }

        public static List<T> ToList<T>(IEnumerable<T> enumerable)
        {
            return enumerable is List<T> ? (List<T>) enumerable : new List<T>(enumerable);
        }
    }
}