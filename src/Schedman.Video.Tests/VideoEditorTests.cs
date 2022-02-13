using Microsoft.VisualStudio.TestTools.UnitTesting;
using VkSchedman.Video;

namespace Schedman.Video.Tests
{
    [TestClass]
    public class VideoEditorTests
    {
        [TestMethod]
        public void ConvertToAviTests()
        {
            new VideoEditor().TestConvertToAvi();
            Assert.IsTrue(true);
        }
    }
}