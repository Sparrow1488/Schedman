using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScheduleVkManager.Entities;
using ScheduleVkManager.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScheduleVkManager.Tests
{
    [TestClass]
    public class SchedulerTests
    {
        private List<TimeSpan> _times;

        public SchedulerTests()
        {
            _times = new List<TimeSpan>()
            {
                new TimeSpan(0, 0, 0),
                new TimeSpan(5, 0, 0),
                new TimeSpan(9, 0, 0),
                new TimeSpan(18, 0, 0),
                new TimeSpan(21, 0, 0)
            };
        }

        [TestMethod]
        public void Create_For2DaysUsingStartTime_9TimesReturned()
        {
            var scheduler = new Scheduler();
            var expired = CreateExpiredTimesListFor2DaysUsingStartTime();

            scheduler.Create(_times, 2, new DateTime(2029, 1, 1));
            var result = scheduler.Times.ToList();
            CollectionAssert.AreEqual(expired, result);
        }

        [TestMethod]
        public void Create_LittleStartTime_InvalidException()
        {
            var scheduler = new Scheduler();
            Assert.ThrowsException<InvalidInputDateException>(() => 
                                            scheduler.Create(_times, 100, new DateTime(2000, 1, 1)));
        }

        [TestMethod]
        public void Create_UsingIntervalInTwoHoursAnd12MaxCount_12Returned()
        {
            var scheduler = new Scheduler();
            var first = new DateTime(2029, 02, 13);
            var interval = new TimeSpan(2, 0, 0);
            var count = 12;
            var expired = CreateExpiredTimesUsingIntervalInTwoHoursAnd12MaxCount();

            scheduler.Create(first, interval, count);
            var result = scheduler.Times.ToList();

            CollectionAssert.AreEqual(expired, result);
        }

        [TestMethod]
        public void Create_NullCollection_ArgumentNullException()
        {
            var scheduler = new Scheduler();
            Assert.ThrowsException<ArgumentNullException>(() => 
                                                scheduler.Create(null));
        }

        [TestMethod]
        public void Create_UsingNegativeDaysValue_ArgumentException()
        {
            var scheduler = new Scheduler();
            var times = new List<TimeSpan>() { new TimeSpan(2, 0, 0) };
            Assert.ThrowsException<ArgumentException>(() =>
                                    scheduler.Create(times, -2, null));
        }

        [TestMethod]
        public void Create_UsingNegativeLimitCountValue_ArgumentException()
        {
            var scheduler = new Scheduler();
            Assert.ThrowsException<ArgumentException>(() => 
                                    scheduler.Create(null, 12, -1, null));
        }


        #region Prepared Expired Results
        private List<DateTime> CreateExpiredTimesListFor2DaysUsingStartTime()
        {
            return new List<DateTime>()
            {
                new DateTime(2029, 1, 1, 5, 0, 0),
                new DateTime(2029, 1, 1, 9, 0, 0),
                new DateTime(2029, 1, 1, 18, 0, 0),
                new DateTime(2029, 1, 1, 21, 0, 0),
                new DateTime(2029, 1, 2, 0, 0, 0),
                new DateTime(2029, 1, 2, 5, 0, 0),
                new DateTime(2029, 1, 2, 9, 0, 0),
                new DateTime(2029, 1, 2, 18, 0, 0),
                new DateTime(2029, 1, 2, 21, 0, 0),
            };
        }

        private List<DateTime> CreateExpiredTimesUsingIntervalInTwoHoursAnd12MaxCount()
        {
            return new List<DateTime>()
            {
                new DateTime(2029, 2, 13, 0, 0, 0),
                new DateTime(2029, 2, 13, 2, 0, 0),
                new DateTime(2029, 2, 13, 4, 0, 0),
                new DateTime(2029, 2, 13, 6, 0, 0),
                new DateTime(2029, 2, 13, 8, 0, 0),
                new DateTime(2029, 2, 13, 10, 0, 0),
                new DateTime(2029, 2, 13, 12, 0, 0),
                new DateTime(2029, 2, 13, 14, 0, 0),
                new DateTime(2029, 2, 13, 16, 0, 0),
                new DateTime(2029, 2, 13, 18, 0, 0),
                new DateTime(2029, 2, 13, 20, 0, 0),
                new DateTime(2029, 2, 13, 22, 0, 0),
            };
        }

        #endregion

    }
}
