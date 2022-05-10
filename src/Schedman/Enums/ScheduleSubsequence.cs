using System;

namespace Schedman.Enums
{
    public class ScheduleSubsequence
    {
        internal ScheduleSubsequence() { }

        internal TimeSpan Every;

        private static ScheduleSubsequence EveryHour(int hour)
        {
            return new ScheduleSubsequence()
            {
                Every = new TimeSpan(hour)
            };
        }
    }
}
