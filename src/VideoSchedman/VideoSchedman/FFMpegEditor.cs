using Newtonsoft.Json;
using Serilog;
using System.Text;
using VideoSchedman.Abstractions;
using VideoSchedman.Entities;
using VideoSchedman.Enums;
using static VideoSchedman.Abstractions.IVideoEditor;

namespace VideoSchedman
{
    public class FFMpegEditor : IVideoEditor
    {
        public FFMpegEditor(string ffmpegPath = "")
        {
            _config = new Configuration();
            _scriptBuilder = new ScriptBuilder();
            _executableProcess = string.IsNullOrWhiteSpace(ffmpegPath) ? 
                                    new ExecutableProcess().FilePathFromConfig("ffmpegPath") : new ExecutableProcess(ffmpegPath);
            if (Project.IsAvailable)
                _projectName = Project.Name;
            else _projectName = Project.CreateProject();

            _jsonSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
        }

        private Configuration _config;
        private IScriptBuilder _scriptBuilder;
        private IExecutableProcess _executableProcess;
        private JsonSerializerSettings _jsonSettings;
        private string _projectName;

        public event LogAction OnCachedSource;
        public event LogAction OnConvertedSource;

        public IVideoEditor Configure(Action<Configuration> configBuilder)
        {
            if(configBuilder is null)
                throw new ArgumentNullException($"{nameof(configBuilder)} cannot be null!");
            _config = _config ?? new Configuration();
            configBuilder(_config);
            SaveFilesMeta();
            return this;
        }

        public async Task ConcatSourcesAsync(ConcatType concatType)
        {
            _scriptBuilder.Clean();
            var notCached = _config.Sources.Where(src => string.IsNullOrWhiteSpace(src.Links.Converted));
            foreach (var src in notCached)
                await CacheSource(src, _config.OutputFile.VideoQuality);
            var cachedFiles = GetCachedCopies().ToArray();
            if (cachedFiles.Length < 1) throw new Exception("No cached files to processing");
            if (concatType == ConcatType.ReencodingConcat || concatType == ConcatType.ReencodingConcatConvertedViaTransportStream)
            {
                if (concatType == ConcatType.ReencodingConcatConvertedViaTransportStream)
                {
                    CleanTsCache();
                    foreach (var source in _config.Sources)
                        await ConvertToTsFormatAsync(source);
                    cachedFiles = GetCachedTsCopies().ToArray();
                } 
                var script = _scriptBuilder.ConfigureInputs(cmd => cmd.Add("-y -f concat -safe 0 -err_detect ignore_err"))
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
                var script = _scriptBuilder.ConfigureInputs(cmd => cmd.Add("-y"))
                                           .ConfigureOutputs(cmd => cmd.Add($"-filter_complex \"{filterComplexArgs}\" -c:a copy -c:v libx264 -preset fast -vsync cfr -r 45"))
                                           .Build(_config, format => format.CombineSources(cachedFiles));
                await _executableProcess.StartAsync(script);
            }
            if (concatType == ConcatType.Demuxer)
            {
                throw new NotImplementedException();
            }
        }

        private async Task CacheSource(FileMeta fileMeta, VideoQuality quality)
        {
            if (!fileMeta.Analyse.WithAudio())
                await PutSilentOnVideoAsync(fileMeta);

            var config = new Configuration();
            string cacheDirPath = Paths.ConvertedFiles.Path;
            var cachedFilesCount = Directory.GetFiles(cacheDirPath).Length;
            config.AddSrc(fileMeta.ToString())
                  .SaveTo($"video{cachedFilesCount+1}", cacheDirPath)
                  .SaveAs("mp4")
                  .Quality(quality);

            var scriptBuilder = new ScriptBuilder();
            scriptBuilder.ConfigureInputs(commands => commands.Add($"-y -i {Paths.Resources}/black{config.OutputFile.VideoQuality}.png"));
            scriptBuilder.ConfigureOutputs(commands => commands.Add($"-filter_complex \"[1:v]scale={quality.Width}:-1[v2];[0:v][v2]overlay=(main_w - overlay_w)/2:(main_h - overlay_h)/2\" -vsync cfr -qscale:v 2 -preset fast -crf 18 -r 40"));
            var script = scriptBuilder.Build(config);
            Log.Debug($"Кэшируем файл {fileMeta.Name}...");
            await _executableProcess.StartAsync(script);
            if (!File.Exists(config.OutputFile.ToString()))
                throw new Exception("Failed caching file!");
            fileMeta.Links.Converted = config.OutputFile.ToString();
            Log.Debug($"Кэширован файл {fileMeta.Name}");
            OnCachedSource?.Invoke($"file: {fileMeta.Name} was cached");
            
            SaveFilesMeta();
        }

        private async Task ConvertToTsFormatAsync(FileMeta file)
        {
            var scriptBuilder = new ScriptBuilder();
            var tsCachedFilesDir = Paths.TsFiles.Path;
            if (!Directory.Exists(tsCachedFilesDir))
                Directory.CreateDirectory(tsCachedFilesDir);
            var tsFileCount = Directory.GetFiles(tsCachedFilesDir).Length;
            var config = new Configuration()
                            .AddSrc(file.Links.Converted)
                            .SaveTo($"videoTransportStream({tsFileCount})", tsCachedFilesDir)
                            .SaveAs("ts");
            var command = scriptBuilder.ConfigureInputs(commands => commands.Add("-y"))
                                        .ConfigureOutputs(commands => commands.Add("-acodec copy -vcodec copy -vbsf h264_mp4toannexb -f mpegts"))
                                        .Build(config);
            file.Links.Ts = config.OutputFile.ToString();
            Log.Debug("Конвертируем файлы в формат .ts");
            await _executableProcess.StartAsync(command);
            scriptBuilder.Clean();
            Log.Debug($"Файл: {new string(Path.GetFileName(file.ToString()))} успешно конвертирован в формат .ts");
            OnConvertedSource?.Invoke($"file: {new string(Path.GetFileName(file.ToString()))} was converted to .ts");
            
            SaveFilesMeta();
        }

        private void CleanTsCache()
        {
            Log.Debug("Очищаем файлы кэшей с расширением .ts");
            var tsCacheFiles = Paths.TsFiles.Path;
            var files = Directory.GetFiles(tsCacheFiles);
            foreach (var file in files)
                File.Delete(file);
        }

        private IEnumerable<FileMeta> GetCachedCopies()
        {
            var projectCachedFilesPath = Paths.ConvertedFiles.Path;
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
            var projectCachedFilesPath = Paths.TsFiles.Path;
            var files = Directory.GetFiles(projectCachedFilesPath);
            var cached = new List<FileMeta>();
            foreach (var file in files)
                cached.Add(FileMeta.From(file));
            return cached;
        }

        private void SaveFilesMeta()
        {
            var metaPath = Paths.CurrentProject + "/meta.txt";
            if(File.Exists(metaPath))
                File.Delete(metaPath);
            using (var sw = File.CreateText(metaPath))
            {
                var json = JsonConvert.SerializeObject(_config.Sources, _jsonSettings);
                sw.WriteLine(json);
            }
            Log.Debug("Сохранены мета файлы");
        }

        private async Task PutSilentOnVideoAsync(FileMeta fileMeta)
        {
            Log.Debug("Добавляем аудиодорожку немому видео");
            string endPattern = "(silent)";
            string oldName = fileMeta.Name;
            string newFileName = $"{oldName}{endPattern}";
            await _executableProcess.StartAsync($"-y -f lavfi -i anullsrc=channel_layout=stereo:sample_rate=44100 -i \"{fileMeta}\" -c:v copy -c:a aac -shortest \"{fileMeta.RootPath}/{newFileName}.{fileMeta.Extension}\"");
            fileMeta.Name = newFileName;
            File.Delete($"{fileMeta.RootPath}/{oldName}.{fileMeta.Extension}");
        }
    }
}