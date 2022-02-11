using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScheduleVkManager.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection)
        {
            Random rnd = new Random();
            collection = collection.OrderBy(x => rnd.Next()).ToArray();
            return collection;
        }

        public static void PrintErrors(this IEnumerable<string> errorCollection)
        {
            foreach (var error in errorCollection)
            {
                Log.Error(error);
            }
        }
    }
}
