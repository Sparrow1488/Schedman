using Serilog;
using VideoSchedman;
using VideoSchedman.Enums;

Log.Logger = new LoggerConfiguration()
              .MinimumLevel
              .Debug()
              .WriteTo.Console()
              .CreateLogger();

string ffmpeg = @"D:\games\ffmpeg\ffmpeg-master-latest-win64-gpl-shared\bin\ffmpeg.exe";
string rootVideos = @"C:\Users\aleks\Downloads\test-videos-2";
var files = Directory.GetFiles(rootVideos).ToList();
string resultPath = string.Empty;

Log.Information("Запускаем");

var editor = new FFMpegEditor(ffmpeg).Configure(config =>
{
    files.ForEach(file => config.AddSrc(file));
    config.SaveTo("concated-endoded-video-result")
          .Quality(VideoQuality.Preview);
    resultPath = config.OutputFile.ToString();
});

Log.Information($"Добавлены файлы из папки \"{rootVideos}\" ({files.Count})");

await editor.ConcatSourcesAsync(ConcatType.ReencodingComplexFilter);
Log.Information("Успешно");