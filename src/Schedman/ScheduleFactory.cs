using Schedman.Enums;
using Schedman.Exceptions;
using Schedman.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Schedman
{
    public static class ScheduleFactory
    {
        public static Schedule CreateFromTimesSchedule(IEnumerable<TimeSpan> times)
        {
            return new Schedule(times)
            {
                Dates = CreateDates(times, 30)
            };
        }

        public static Schedule CreateFromSubsequenceSchedule(ScheduleSubsequence subsequence)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<DateTime> CreateDates(IEnumerable<TimeSpan> times, int days, DateTime? startTime = null)
        {
            if (times is null)
                throw new ArgumentNullException($"{nameof(times)} was null");
            if (days < 1)
                throw new ArgumentException($"{nameof(days)} should be possitive! (more zero)");

            var timesArr = times.ToArray();
            var actual = times.GetActualTime();

            if (startTime != null && startTime < DateTime.Now)
                throw new SchedmanInvalidInputDateException("You can't create scheduler with start date in the past!");

            var resultTimes = new List<DateTime>();
            var now = startTime ?? DateTime.Now;
            bool checkFirstDay = false;
            for (int j = 0; j < days; j++)
            {
                for (int i = 0; i < timesArr.Length; i++)
                {
                    DateTime scheduleTime;
                    if (!checkFirstDay)
                    {
                        var index = Array.IndexOf(timesArr, actual);
                        if (index != 0) i = index;
                        checkFirstDay = true;
                    }
                    scheduleTime = new DateTime(now.Year, now.Month, now.Day, timesArr[i].Hours, timesArr[i].Minutes, timesArr[i].Seconds);
                    scheduleTime = scheduleTime.AddDays(j);
                    resultTimes.Add(scheduleTime);
                }
            }
            return resultTimes;
        }
    }
}
