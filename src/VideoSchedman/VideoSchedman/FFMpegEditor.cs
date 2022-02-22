using System.Text;
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

            var projectPath = $"{Paths.FilesCache}/{_projectName}";
            if (!Directory.Exists(projectPath))
                Directory.CreateDirectory(projectPath);
        }

        private Configuration _config;
        private IScriptBuilder _scriptBuilder;
        private IExecutableProcess _executableProcess;
        //private string _projectName = $"project_{Guid.NewGuid()}";
        private string _projectName = $"project_c6149ba7-3dcf-4e57-b85b-a682eebfc781";

        public IVideoEditor Configure(Action<Configuration> configBuilder)
        {
            if(configBuilder is null)
                throw new ArgumentNullException($"{nameof(configBuilder)} cannot be null!");
            _config = _config ?? new Configuration();
            configBuilder(_config);
            return this;
        }

        public async Task ConcatSourcesAsync(ConcatType concatType)
        {
            _scriptBuilder.Clean();
            //foreach (var src in _config.Sources)
            //    await CacheSource(src, _config.OutputFile.VideoQuality);
            //var cachedFiles = GetCachedCopies().ToArray();
            //await CacheAsTsFormatAsync();
            var cachedFiles = GetCachedCopies().ToArray();
            if (cachedFiles.Length < 1) throw new Exception("No cached files to processing");
            if (concatType == ConcatType.ReencodingConcat || concatType == ConcatType.ReencodingConcatConvertedViaTransportStream)
            {
                if(concatType == ConcatType.ReencodingConcatConvertedViaTransportStream)
                {
                    await CacheAsTsFormatAsync();
                    cachedFiles = GetCachedTsCopies().ToArray();
                }
                var script = _scriptBuilder.ConfigureInputs(cmd => cmd.Add("-f concat -safe 0 -err_detect ignore_err"))
                                           .ConfigureOutputs(cmd => cmd.Add("-c:a copy -c:v copy -preset fast -vsync cfr -r 45"))
                                           .Build(_config, format => format.CombineSourcesInTxt(cachedFiles));
                await _executableProcess.StartAsync(script);
            }
            if (concatType == ConcatType.ReencodingComplexFilter)
            {
                throw new NotImplementedException("Please, dont use this concationation type, it while not work correct");
                var filterComplexArgs = new StringBuilder();
                cachedFiles.ToList().ForEach(file => filterComplexArgs.Append($"[{Array.IndexOf(cachedFiles, file)}:v]"));
                filterComplexArgs.Append($"concat=n={cachedFiles.Length}");
                var script = _scriptBuilder.ConfigureOutputs(cmd => cmd.Add($"-filter_complex \"{filterComplexArgs}\" -c:a copy -c:v libx264 -preset fast -vsync cfr -r 45"))
                                           .Build(_config, format => format.CombineSources(cachedFiles));
                await _executableProcess.StartAsync(script);
            }
        }

        #region TO REMOVE
        public Task ConcatSourcesAsync()
        {
            _scriptBuilder.Clean();
            var cachedFiles = GetCachedCopies();

            var script = _scriptBuilder?.ConfigureInputs(commands =>
            {
                commands.Add("-y");
                commands.Add("-f concat");
                commands.Add("-safe 0");
            })
            .ConfigureOutputs(commands =>
            {
                //commands.Add("-c copy");
                //commands.Add("-c:a aac -c:v copy");
                //commands.Add("-vf scale=1920x1080:force_original_aspect_ratio=decrease -c:a copy -c:v libx264");
                commands.Add("-c:a aac -c:v libx264");
            })
            .ChangeFormat(format => format.CombineSourcesInTxt(cachedFiles))
            .Build(_config);
            return _executableProcess.StartAsync(script ?? string.Empty);
        }
        #endregion

        private async Task CacheSource(FileMeta fileMeta, VideoQuality quality)
        {
            var config = new Configuration();
            string cacheDirPath = Path.Combine(Paths.FilesCache.Path, _projectName);
            var cachedFilesCount = Directory.GetFiles(cacheDirPath).Length;
            config.AddSrc(fileMeta.ToString())
                  .SaveTo($"video{cachedFilesCount+1}", cacheDirPath)
                  .SaveAs("mp4")
                  .Quality(quality);
            var scriptBuilder = new ScriptBuilder();
            scriptBuilder.ConfigureInputs(commands => commands.Add($"-y -i {Paths.Resources}/black1920x1080.png"));
            scriptBuilder.ConfigureOutputs(commands => commands.Add($"-filter_complex \"[1:v]scale={quality.Width}:-1[v2];[0:v][v2]overlay=(main_w - overlay_w)/2:(main_h - overlay_h)/2\" -vsync cfr -qscale:v 2 -preset fast -crf 16 -r 45"));
            var script = scriptBuilder.Build(config);
            await _executableProcess.StartAsync(script);
            if (!File.Exists(config.OutputFile.ToString()))
                throw new Exception("Failed caching file!");
        }

        public async Task CacheAsTsFormatAsync()
        {
            int counter = 0;
            var scriptBuilder = new ScriptBuilder();
            var tsCachedFilesDir = Path.Combine(Paths.FilesCache.ToString(), _projectName, "ts");
            if (!Directory.Exists(tsCachedFilesDir))
                Directory.CreateDirectory(tsCachedFilesDir);
            var existsCachedFiles = Directory.GetFiles(Path.Combine(Paths.FilesCache.ToString(), _projectName));
            CleanTsCache();
            foreach (var src in existsCachedFiles)
            {
                var config = new Configuration()
                             .AddSrc(src.ToString())
                             .SaveTo($"videoTransportStream({counter})", tsCachedFilesDir)
                             .SaveAs("ts");
                var command = scriptBuilder.ConfigureInputs(commands => commands.Add("-y"))
                                           .ConfigureOutputs(commands => commands.Add("-acodec copy -vcodec copy -vbsf h264_mp4toannexb -f mpegts"))
                                           .Build(config);
                await _executableProcess.StartAsync(command);
                if(!File.Exists(src.ToString()))
                    Console.WriteLine("Не кэшировано");
                scriptBuilder.Clean();
                counter++;
            }
        }

        public void CleanTsCache()
        {
            var tsCacheFiles = Path.Combine(Paths.FilesCache.ToString(), _projectName, "ts");
            var files = Directory.GetFiles(tsCacheFiles);
            foreach (var file in files)
                File.Delete(file);
        }

        private IEnumerable<FileMeta> GetCachedCopies()
        {
            var projectCachedFilesPath = Path.Combine(Paths.FilesCache.Path, _projectName);
            var files = Directory.GetFiles(projectCachedFilesPath);
            var cached = new List<FileMeta>();
            foreach (var file in files)
            {
                cached.Add(FileMeta.From(file));
            }
            return cached;
        }

        private IEnumerable<FileMeta> GetCachedTsCopies()
        {
            var projectCachedFilesPath = Path.Combine(Paths.FilesCache.Path, _projectName, "ts");
            var files = Directory.GetFiles(projectCachedFilesPath);
            var cached = new List<FileMeta>();
            foreach (var file in files)
                cached.Add(FileMeta.From(file));
            return cached;
        }


    }
}