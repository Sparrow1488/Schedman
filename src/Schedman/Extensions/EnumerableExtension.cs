using System;
using System.Collections.Generic;
using System.Linq;

namespace Schedman.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection)
        {
            Random rnd = new Random();
            collection = collection.OrderBy(x => rnd.Next()).ToArray();
            return collection;
        }
    }
}
