using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using VkSchedman.Video.Options;

namespace Schedman.Video.Tests
{
    [TestClass]
    public class InputOptionsTests
    {
        [TestMethod]
        public void AddSource_NullValue_ExceptionReturned()
        {
            var options = new InputOptions();
            Assert.ThrowsException<ArgumentException>(() =>
                                            options.AddSource(null));
        }

        private static readonly string _testInputPath = "C://Program/directory/video.mp4";

        [TestMethod]
        public void Build_SetVideoSource_CommandReturned()
        {
            var options = new InputOptions();
            string expected = $"-i \"{_testInputPath}\"";

            options.AddSource(_testInputPath);
            var command = options.Build();

            Assert.AreEqual(expected, command);
        }

        [TestMethod]
        public void Build_SetSourceStartAndStartPos_CommandReturned()
        {
            var options = new InputOptions();
            string expected = $"-ss 00:00:05 -i \"{_testInputPath}\"";

            options.AddSource(_testInputPath);
            options.SetStartPosition(new TimeSpan(0, 0, 5));
            var command = options.Build();

            Assert.AreEqual(expected, command);
        }

        [TestMethod]
        public void Build_SetSourceStartAndEndPos_CommandReturned()
        {
            var options = new InputOptions();
            string expected = $"-t 00:00:25 -i \"{_testInputPath}\"";

            options.AddSource(_testInputPath);
            options.SetEndPosition(new TimeSpan(0, 0, 25));
            var command = options.Build();

            Assert.AreEqual(expected, command);
        }

        [TestMethod]
        public void Build_SetSourceAndStartPosAndEndPos_CommandReturned()
        {
            var options = new InputOptions();
            string expected = $"-ss 00:00:05 -t 00:00:25 -i \"{_testInputPath}\"";

            options.AddSource(_testInputPath);
            options.SetStartPosition(new TimeSpan(0, 0, 5));
            options.SetEndPosition(new TimeSpan(0, 0, 25));
            var command = options.Build();

            Assert.AreEqual(expected, command);
        }

        
    }
}
