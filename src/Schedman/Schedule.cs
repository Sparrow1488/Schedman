using System;
using System.Collections.Generic;

namespace Schedman
{
    public class Schedule
    {
        internal Schedule(IEnumerable<TimeSpan> dayTimes) 
        { 
            DayTimes = dayTimes;
        }

        public IEnumerable<TimeSpan> DayTimes { get; }
        public IEnumerable<DateTime> Dates { get; internal set; }

        public IEnumerable<DateTime> TakeDays(int days)
        {
            return new List<DateTime>();
        }
    }
}
