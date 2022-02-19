using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using VideoSchedman.Entities;

namespace VideoSchedman.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void AddSrc_ValidVideoPath_SuccessAddedInCollection()
        {
            var config = new Configuration();
            var filePath = @"D:\games\ffmpeg\video.avi";
            string expectedFileName = "video";
            string expectedFileExtension = "avi";
            string expectedRootPath = @"D:\games\ffmpeg";

            config.AddSrc(filePath);
            var addedFile = config.Sources.FirstOrDefault();

            Assert.AreEqual(expectedFileName, addedFile.Name);
            Assert.AreEqual(expectedFileExtension, addedFile.Extension);
            Assert.AreEqual(expectedRootPath.Replace("/", "\\"), addedFile.RootPath);
        }

        [TestMethod]
        public void AddSrc_NotExistsFile_FileNotFoundException()
        {
            var config = new Configuration();
            Assert.ThrowsException<FileNotFoundException>(() => config.AddSrc(@"D:\games\ffmpg\video.vi"));
        }

        [TestMethod]
        public void SaveTo_NotDirectoryExistsPath_DirectoryNotFoundException()
        {
            var config = new Configuration();
            Assert.ThrowsException<DirectoryNotFoundException>(() => config.SaveTo(@"D:\gam42s", "name"));
        }

        [TestMethod]
        public void SaveTo_DirPathOrNameIsNullOrEmpty_ArgumentException()
        {
            var config = new Configuration();
            Assert.ThrowsException<ArgumentException>(() => config.SaveTo(null, ""));
        }

        [TestMethod]
        public void SaveTo_FilePath_ValidMetaFile()
        {
            var config = new Configuration();
            var expectedName = "[TEST]-Output-Video_File";
            var expectedDirPath = @"D:\games\ffmpeg";

            config.SaveTo(expectedDirPath, expectedName);
            var @out = config.OutputFile;

            Assert.AreEqual(expectedDirPath, @out.RootPath);
            Assert.AreEqual(expectedName, @out.Name);
        }

        [TestMethod]
        public void Quality_Null_ArgumentException()
        {
            var config = new Configuration();
            Assert.ThrowsException<ArgumentException>(() => config.Quality(null));
        }

        [TestMethod]
        public void SaveAs_Null_ArgumentException()
        {
            var config = new Configuration();
            Assert.ThrowsException<ArgumentException>(() => config.SaveAs(null));
        }

    }
}