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

        public void Create(IEnumerable<TimeSpan> times, int count)
        {
            if (times is null) {
                throw new ArgumentNullException($"{nameof(times)} was null");
            }

            var timesArr = times.ToArray();
            var actual = times.GetActualTime();
            for (int i = 0; i < count; i++) {
                //((IList<DateTime>)Times).Add(new DateTime(now.Ticks));
            }
        }

        
    }
}
