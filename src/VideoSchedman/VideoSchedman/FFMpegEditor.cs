using VideoSchedman.Abstractions;
using VideoSchedman.Entities;
using VideoSchedman.Enums;

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
        private List<string> _filesCopyAsTs = new List<string>();
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
            {
                commands.Add("-r 30 -b:v 3M");
            })
            .ChangeFormat(format => format.CombineSources(_config.Sources))
            .Build(_config);

            if (File.Exists(_config.OutputFile.ToString()))
            {
                Console.WriteLine("Удаляю существующий файл");
                File.Delete(_config.OutputFile.ToString());
            }

            return _executableProcess.StartAsync(script ?? string.Empty);
        }

        public async Task CacheAsTsFormatAsync()
        {
            int counter = 0;
            var scriptBuilder = new ScriptBuilder();
            foreach (var src in _config.Sources)
            {
                var config = new Configuration()
                             .AddSrc(src.ToString())
                             .SaveTo(Paths.FilesCache.Path, $"video({counter})")
                             .SaveAs("ts");
                var command = scriptBuilder.ConfigureOutputs(commands =>
                                            commands.Add("-acodec copy -vcodec copy -vbsf h264_mp4toannexb -f mpegts"))
                                           .ConfigureInputs(commands => commands.Add("-y"))
                                           .Build(config);
                await _executableProcess.StartAsync(command);

                scriptBuilder.Clean();
                counter++;
            }
        }
    }
}