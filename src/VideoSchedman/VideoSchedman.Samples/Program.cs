using Serilog;
using VideoSchedman;

Log.Logger = new LoggerConfiguration()
              .MinimumLevel
              .Debug()
              .WriteTo.Console()
              .CreateLogger();

string ffmpeg = @"D:\games\ffmpeg\ffmpeg.exe";
string rootVideos = @"C:\Users\aleks\Downloads\test-videos-2\";
string video1 = "1.mp4";
string video2 = "2.mp4";
string resultPath = string.Empty;

Log.Information("Запускаем");

var editor = new FFMpegEditor(ffmpeg).Configure(config =>
{
    config.AddSrc(rootVideos + video1)
          .AddSrc(rootVideos + video2)
          .SaveTo("./", "result");
    resultPath = config.OutputFile.ToString();
});
await editor.CacheAsTsFormatAsync();

Log.Debug($"Добавлены файлы из папки \"{rootVideos}\": '{video1}', '{video2}'");

Log.Information("Обрабатываем...");
await editor.ConcatSourcesAsync();
Log.Information("Готово: " + File.Exists(resultPath));
