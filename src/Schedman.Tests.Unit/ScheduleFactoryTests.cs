using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Schedman.Tests.Unit
{
    public class ScheduleFactoryTests
    {
        [Test]
        public void CreateFromTimesSchedule_Times_Success()
        {
            var times = new List<TimeSpan>() {
                new TimeSpan(3, 0, 0),
                new TimeSpan(8, 0, 0),
                new TimeSpan(12, 0, 0),
                new TimeSpan(15, 0, 0)
            };
            var schedule = ScheduleFactory.CreateFromTimesSchedule(times);
            Assert.AreEqual(times, schedule.DayTimes);
        }

        [Test]
        public void CreateFromSubsequenceSchedule_Times_Success()
        {
            var times = new List<TimeSpan>() {
                new TimeSpan(3, 0, 0),
                new TimeSpan(8, 0, 0),
                new TimeSpan(12, 0, 0),
                new TimeSpan(15, 0, 0)
            };
            var schedule = ScheduleFactory.CreateFromTimesSchedule(times);
            Assert.AreEqual(times, schedule.DayTimes);
        }
    }
}