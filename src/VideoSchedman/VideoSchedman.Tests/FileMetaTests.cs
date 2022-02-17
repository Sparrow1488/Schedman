using Microsoft.VisualStudio.TestTools.UnitTesting;
using VideoSchedman.Entities;

namespace VideoSchedman.Tests
{
    [TestClass]
    public class FileMetaTests
    {
        [TestMethod]
        public void From_PathToVideo_ValidMeta()
        {
            string expExtension = "mp4";
            string expName = "1";
            string expRootPath = @"C:\Users\aleks\Downloads\test-videos-2";

            var meta = FileMeta.From(@"C:\Users\aleks\Downloads\test-videos-2\1.mp4");

            Assert.AreEqual(expExtension, meta.Extension);
            Assert.AreEqual(expName, meta.Name);
            Assert.AreEqual(expRootPath, meta.RootPath);
        }
    }
}
