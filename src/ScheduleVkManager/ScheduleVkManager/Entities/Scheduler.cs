using ScheduleVkManager.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleVkManager.Entities
{
    public class Scheduler : IEnumerable<DateTime>
    {
        public IEnumerable<DateTime> Times { get; private set; } = new List<DateTime>();
        public IEnumerator<DateTime> GetEnumerator() => Times.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Times.GetEnumerator();

        public void Create(IEnumerable<DateTime> times)
        {
            Times = times;
        }

        public void Create(DateTime firstTime, TimeSpan interval, int count)
        {
            for (int i = 0; i < count; i++) {
                ((IList<DateTime>)Times).Add(firstTime + interval * i);
            }
        }

        public void Create(IEnumerable<TimeSpan> times, int days)
        {
            if (times is null) {
                throw new ArgumentNullException($"{nameof(times)} was null");
            }

            var timesArr = times.ToArray();
            var actual = times.GetActualTime();
            var now = DateTime.Now;
            for (int j = 0; j < days; j++) {
                for (int i = 0; i < timesArr.Length; i++) {
                    DateTime scheduleTime;
                    if (j == 0) {
                        var index = Array.IndexOf(timesArr, actual);
                        if (index != 0) i = index;
                        else j++;
                    }
                    scheduleTime = new DateTime(now.Year, now.Month, now.Day, timesArr[i].Hours, timesArr[i].Minutes, timesArr[i].Seconds);
                    scheduleTime = scheduleTime.AddDays(j);
                    ((IList<DateTime>)Times).Add(scheduleTime);
                }
            }
        }

        public void Create(IEnumerable<TimeSpan> times, int days, int limitCount)
        {
            Create(times, days);
            if(Times.Count() > limitCount) {
                var timesArr = Times.ToArray();
                Array.Resize(ref timesArr, limitCount);
                Times = timesArr;
            }
        }
    }
}
