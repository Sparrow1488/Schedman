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
        private IScriptBuilder _scriptBuilder;
        private IExecutableProcess _executableProcess;

        public IVideoEditor Configure(Action<Configuration> configBuilder)
        {
            if(configBuilder is null)
                throw new ArgumentNullException($"{nameof(configBuilder)} cannot be null!");
            _config = _config ?? new Configuration();
            configBuilder(_config);
            return this;
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
            .ChangeFormat(format => format.CombineSources(cachedFiles))
            .Build(_config);
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
                             .SaveTo($"{src.Name}({counter})", Paths.FilesCache.Path)
                             //.SaveAs("ts");
                             .SaveAs("mp4");
                var command = scriptBuilder.ConfigureOutputs(commands =>
                {
                    //commands.Add("-c:v libx264 -preset slow -crf 22 -level 4.1 -threads 0 -c:a aac");
                    //commands.Add("-acodec copy -vcodec copy -vbsf h264_mp4toannexb -f mpegts");
                    commands.Add("-filter_complex \"[1:v]scale=1920:-1[v2];[0:v][v2]overlay=(main_w - overlay_w)/2:(main_h - overlay_h)/2\" -preset slow -crf 10 -r 45");
                    //commands.Add("-c:v vp9 -c:a aac");
                }).ConfigureInputs(commands =>
                {
                    commands.Add("-i D:/games/ffmpeg/ffmpeg-master-latest-win64-gpl-shared/bin/black.png");
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
            var files = Directory.GetFiles(Paths.FilesCache.Path);
            var cached = new List<FileMeta>();
            foreach (var file in files)
            {
                cached.Add(FileMeta.From(file));
            }
            return cached;
        }

    }
}