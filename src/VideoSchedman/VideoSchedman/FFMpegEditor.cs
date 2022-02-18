using VideoSchedman.Abstractions;
using VideoSchedman.Entities;

namespace VideoSchedman
{
    public class FFMpegEditor : IVideoEditor
    {
        public FFMpegEditor(string ffmpegPath)
        {
            _config = new Configuration();
            _scriptBuilder = new ScriptBuilder();
            _executableProcess = new ExecutableProcess(ffmpegPath);
        }

        private Configuration _config;
        private IScriptBuilder _scriptBuilder;
        private IExecutableProcess _executableProcess;

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
            var script = _scriptBuilder?.ConfigureInputs(commands =>
            {
                commands.Add("-y");
                commands.Add("-f concat");
                commands.Add("-safe 0");
            })
            .ConfigureOutputs(commands =>
                commands.Add("-vf mpdecimate,setpts=N/FRAME_RATE/TB"))
            .ChangeFormat(format => format.CombineSources(_config.Sources))
            .Build(_config);
            return _executableProcess.StartAsync(script ?? string.Empty);
        }
    }
}