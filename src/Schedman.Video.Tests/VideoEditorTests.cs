using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            _input.AddSource("D:/games/ffmpeg/video.mp4");
            _output = new OutputOptions();
            _output.SetOutputPath("C:/Users/aleks/Downloads");
            _output.SetOutputFileName("test-video");
        }

        private IInputOptions _input;
        private IOutputOptions _output;
        private static string _ffmpegPath = @"D:/games/ffmpeg/ffmpeg.exe";
        private static readonly string _testFilePath = "C://Program/directory/video.mp4";

        [TestMethod]
        public void ConvertToExtensionTests()
        {
            var editor = new VideoEditor(_ffmpegPath);
            editor.SetOptions(_input);
            editor.SetOptions(_output);
            editor.ConvertToExtension(FileExtension.AVI);
            var result = File.Exists(_output.GetResultPath());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SetOptions_NullValue_ArgumentNullExceptionReturned()
        {
            var editor = new VideoEditor(_ffmpegPath);
            InputOptions input = null;
            OutputOptions output = null;

            Assert.ThrowsException<ArgumentNullException>(() => editor.SetOptions(input));
            Assert.ThrowsException<ArgumentNullException>(() => editor.SetOptions(output));
        }
    }
}