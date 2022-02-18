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
            _config = _config ?? new Configuration();
            configBuilder(_config);
            return this;
        }

        public Task ConcatSourcesAsync()
        {
            _scriptBuilder?.ConfigureInputs(commands =>
            {
                commands.Add("-f concat");
                commands.Add("-safe 0");
            });
            throw new NotImplementedException();
        }
    }
}