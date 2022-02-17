using VideoSchedman.Abstractions;
using VideoSchedman.Entities;

namespace VideoSchedman
{
    public class FFMpegEditor : IVideoEditor
    {
        public FFMpegEditor()
        {
            _config = new Configuration();
            _scriptBuilder = new ScriptBuilder();
            _executableProcess = new ExecutableProcess();
        }

        private Configuration? _config;
        private IScriptBuilder? _scriptBuilder;
        private IExecutableProcess? _executableProcess;

        public IVideoEditor Configure(Action<Configuration> configBuilder)
        {
            if(configBuilder is null)
                throw new ArgumentNullException(nameof(configBuilder));
            if (configBuilder is null)
                _config = new Configuration();
            configBuilder(_config);
            return this;
        }

        public Task ConcatSourcesAsync()
        {
            throw new NotImplementedException();
        }
    }
}