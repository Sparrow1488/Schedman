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
        private string _projectName = $"project_{Guid.NewGuid()}";
        //private string _projectName = $"project_02fbf88b-b142-41be-8af6-5e1240405c21";

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
            if(concatType == ConcatType.ReencodingConcat)
            {
                foreach (var src in _config.Sources)
                    await CacheSource(src, _config.OutputFile.VideoQuality);
                var cachedFiles = GetCachedCopies();
                var script = _scriptBuilder.ConfigureInputs(cmd => cmd.Add("-f concat -safe 0"))
                                           .ConfigureOutputs(cmd => cmd.Add("-c:a copy -c:v libx264 -preset fast -vsync cfr -r 45"))
                                           .Build(_config, format => format.CombineSourcesInTxt(cachedFiles));
                await _executableProcess.StartAsync(script);
            }
        }

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
            scriptBuilder.ConfigureOutputs(commands => commands.Add("-filter_complex \"[1:v]scale=1920:-1[v2];[0:v][v2]overlay=(main_w - overlay_w)/2:(main_h - overlay_h)/2\" -vsync cfr -profile:v high -preset fast -crf 16 -r 45"));
            var script = scriptBuilder.Build(config);
            await _executableProcess.StartAsync(script);
            if (!File.Exists(config.OutputFile.ToString()))
                throw new Exception("Failed caching file!");
        }

        public async Task CacheAsTsFormatAsync()
        {
            int counter = 0;
            var scriptBuilder = new ScriptBuilder();
            foreach (var src in _config.Sources)
            {
                var config = new Configuration()
                             .AddSrc(src.ToString())
                             .SaveTo($"{src.Name}({counter})", Paths.FilesCache.Path)
                             //.SaveAs("ts");
                             .SaveAs("mp4");
                var command = scriptBuilder.ConfigureOutputs(commands =>
                {
                    //commands.Add("-c:v libx264 -preset slow -crf 22 -level 4.1 -threads 0 -c:a aac");
                    //commands.Add("-acodec copy -vcodec copy -vbsf h264_mp4toannexb -f mpegts");
                    commands.Add("-filter_complex \"[1:v]scale=1920:-1[v2];[0:v][v2]overlay=(main_w - overlay_w)/2:(main_h - overlay_h)/2\" -vsync cfr -profile:v high -preset fast -crf 16 -r 45");
                    //commands.Add("-c:v vp9 -c:a aac");
                }).ConfigureInputs(commands =>
                {
                    // TODO: вынести в ресурсы
                    commands.Add($"-i {Paths.Resources}/black1920x1080.png");
                    commands.Add("-y");
                }).Build(config);
                await _executableProcess.StartAsync(command);
                if(!File.Exists(src.ToString()))
                    Console.WriteLine("Не кэшировано");
                scriptBuilder.Clean();
                counter++;
                //./ffmpeg -i "C:\Users\aleks\OneDrive\Desktop\Илья\Repositories\VkSchedman\src\VideoSchedman\VideoSchedman.Samples\bin\Debug\net6.0\meta-files\combined-files_637809563174498961.txt" -filter_complex "concat=n=3:v=0:a=1" -f MOV -vn -y -map "[v]" -map "[a]" output.mp4
            }
        }

        public void CleanCache()
        {
            var files = Directory.GetFiles(Paths.FilesCache.Path);
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

        
    }
}