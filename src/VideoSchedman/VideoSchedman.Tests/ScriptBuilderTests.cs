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
            string rootPath = @"C:\Users\aleks\Downloads\test-videos-2\";
            _config = new Configuration()
                          .AddSrc(rootPath + "1.mp4")
                          .AddSrc(rootPath + "2.mp4");
        }

        private ScriptBuilder _builder;
        private Configuration _config;

        [TestMethod]
        public void Build_InpAndOutConfigurs_ConvertToExtensionCommand()
        {
            var result = _builder.Build(_config);
        }
    }
}
