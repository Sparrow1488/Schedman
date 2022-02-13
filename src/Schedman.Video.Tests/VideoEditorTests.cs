using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using VkSchedman.Video;
using VkSchedman.Video.Abstractions;
using VkSchedman.Video.Enum;
using VkSchedman.Video.Options;

namespace Schedman.Video.Tests
{
    [TestClass]
    public class VideoEditorTests
    {
        public VideoEditorTests()
        {
            _input = new InputOptions();
            _input.AddSource(@"D:\games\ffmpeg\video.mp4");
            _output = new OutputOptions();
            _output.SetOutputPath("C:/Users/aleks/Downloads/out-video.mp4");
        }

        private IInputOptions _input;
        private IOutputOptions _output;
        private static string _ffmpegPath = @"D:\games\ffmpeg\ffmpeg.exe";

        [TestMethod]
        public void ConvertToExtensionTests()
        {
            var editor = new VideoEditor(_ffmpegPath);
            editor.SetOptions(_input);
            editor.SetOptions(_output);
            editor.ConvertToExtension(FileExtension.AVI);
            var result = File.Exists(_output.GetOutputPath());

            Assert.IsTrue(result);
        }
    }
}