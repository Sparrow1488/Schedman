using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using VkSchedman.Video;

namespace Schedman.Video.Tests
{
    [TestClass]
    public class VideoEditorTests
    {
        [TestMethod]
        public void ConvertToAviTests()
        {
            var editor = new VideoEditor(@"D:\games\ffmpeg\ffmpeg.exe");
            editor.OutputFilePath = "C:/Users/aleks/Downloads/out-video.mp4";
            editor.AddVideo(@"D:\games\ffmpeg\video.mp4");
            editor.ConvertToAvi();
            var result = File.Exists(editor.OutputFilePath);

            Assert.IsTrue(result);
        }
    }
}