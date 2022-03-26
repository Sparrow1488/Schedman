using VkSchedman.Exceptions;
using VkSchedman.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VkSchedman.Entities
{
    public class Scheduler : IEnumerable<DateTime>
    {
        public IEnumerable<DateTime> Times { get => _times; }
        private List<DateTime> _times = new List<DateTime>();
        public IEnumerator<DateTime> GetEnumerator() => Times.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Times.GetEnumerator();

        public void Create(IEnumerable<DateTime> times)
        {
            if (times is null)
                throw new ArgumentNullException($"{nameof(times)} can't be a null!");
            if (times.Any(time => time < DateTime.Now))
                throw new InvalidInputDateException("You can't create scheduler with start date in the past!");

            _times = times.ToList();
        }

        public void Create(DateTime firstTime, TimeSpan interval, int count)
        {
            if (count < 0)
                throw new ArgumentException($"{nameof(count)} should be possitive!");
            if (firstTime < DateTime.Now)
                throw new InvalidInputDateException("You can't create scheduler with start date in the past!");

            for (int i = 0; i < count; i++) {
                _times.Add(firstTime + interval * i);
            }
        }

        public void Create(IEnumerable<TimeSpan> times, int days, DateTime? startTime = null)
        {
            if (times is null) 
                throw new ArgumentNullException($"{nameof(times)} was null");
            if (days < 1)
                throw new ArgumentException($"{nameof(days)} should be possitive! (more zero)");

            var timesArr = times.ToArray();
            var actual = times.GetActualTime();

            if (startTime != null && startTime < DateTime.Now)
                throw new InvalidInputDateException("You can't create scheduler with start date in the past!");

            var now = startTime ?? DateTime.Now;
            bool checkFirstDay = false;
            for (int j = 0; j < days; j++) {
                for (int i = 0; i < timesArr.Length; i++) {
                    DateTime scheduleTime;
                    if (!checkFirstDay) {
                        var index = Array.IndexOf(timesArr, actual);
                        if (index != 0) i = index;
                        checkFirstDay = true;
                    }
                    scheduleTime = new DateTime(now.Year, now.Month, now.Day, timesArr[i].Hours, timesArr[i].Minutes, timesArr[i].Seconds);
                    scheduleTime = scheduleTime.AddDays(j);
                    _times.Add(scheduleTime);
                }
            }
        }

        public void Create(IEnumerable<TimeSpan> times, int days, int limitCount, DateTime? startTime = null)
        {
            if (limitCount < 1)
                throw new ArgumentException($"{nameof(limitCount)} should be possitive! (more zero)");

            Create(times, days, startTime);
            if(Times.Count() > limitCount) {
                var timesArr = Times.ToArray();
                Array.Resize(ref timesArr, limitCount);
                _times = timesArr.ToList();
            }
        }
    }
}
