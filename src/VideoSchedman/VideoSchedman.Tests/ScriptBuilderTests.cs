using Microsoft.VisualStudio.TestTools.UnitTesting;
using VideoSchedman.Entities;

namespace VideoSchedman.Tests
{
    [TestClass]
    public class ScriptBuilderTests
    {
        public ScriptBuilderTests()
        {
            _builder = new ScriptBuilder();
            _config = new Configuration()
                          .AddSrc(_rootPath + "1.mp4")
                          .AddSrc(_rootPath + "2.mp4")
                          .SaveTo("./result-dir", "result");
        }

        private string _rootPath = @"C:\Users\aleks\Downloads\test-videos-2\";
        private ScriptBuilder _builder;
        private Configuration _config;

        [TestMethod]
        public void Build_InpAndOutConfigurs_ValidScript()
        {
            var expected = $"-i \"{_rootPath}1.mp4\" -i \"{_rootPath}2.mp4\" \"./result-dir/result.mp4\"".Replace('/', '\\');
            var result = _builder.Build(_config).Replace('/', '\\');

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Build_InpAndOutConfigursAndFormatter_ValidScript()
        {
            var expected = $"-i \"./files-meta/combined-files".Replace('/', '\\');
            var result = _builder.Build(_config, format => 
                                format.CombineSources(_config.Sources)).Replace('/', '\\');

            StringAssert.Contains(result, expected);
            StringAssert.Contains(result, ".txt");
            StringAssert.Contains(result, _config.OutputFile.ToString());
        }

        [TestMethod]
        public void ConfigureInputs_InpAndOutConfigureAndChangedFormatter_ValidScript()
        {
            var builder = new ScriptBuilder();
            builder.ConfigureInputs(commands =>
            {
                commands.Add("-f concat");
                commands.Add("-safe 0");
            });
            var expected = $"-f concat -safe 0 -i \"{_rootPath}1.mp4\" -i \"{_rootPath}2.mp4\" \"./result-dir/result.mp4\"".Replace('/', '\\');

            var result = builder.Build(_config).Replace('/', '\\');

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConfigureOutputs_InpAndOutConfigureAndChangedFormatter_ValidScript()
        {
            var builder = new ScriptBuilder();
            builder.ConfigureOutputs(commands => {
                commands.Add("-c copy");
            });
            var expected = $"-i \"{_rootPath}1.mp4\" -i \"{_rootPath}2.mp4\" -c copy \"./result-dir/result.mp4\"".Replace('/', '\\');

            var result = builder.Build(_config).Replace('/', '\\');

            Assert.AreEqual(expected, result);
        }
    }
}
