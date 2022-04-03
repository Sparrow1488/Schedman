using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schedman.Extensions
{
    public static class ScheduleEnumerableExtension
    {
        /// <returns>First valid date for this day</returns>
        /// <exception cref="InvalidOperationException">Not found time</exception>
        public static TimeSpan GetActualTime(this IEnumerable<TimeSpan> times)
        {
            var today = DateTime.Now;
            var timeNow = new TimeSpan(today.Hour, today.Minute, today.Second);
            var actual = times.Where(time => time > timeNow).FirstOrDefault();
            if (!times.Contains(actual)) {
                actual = times.Where(time => time >= new TimeSpan(0, 0, 0)).First();
            }
            return actual;
        }
    }
}
