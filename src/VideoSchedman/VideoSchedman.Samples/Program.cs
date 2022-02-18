using Serilog;
using VideoSchedman;

Log.Logger = new LoggerConfiguration()
              .MinimumLevel
              .Debug()
              .WriteTo.Console()
              .CreateLogger();

//string ffmpeg = @"D:\games\ffmpeg\ffmpeg.exe";
//string rootVideos = @"C:\Users\aleks\Downloads\test-videos-2\";
//string video1 = "1.mp4";
//string video2 = "2.mp4";

string ffmpeg = @"D:\games\ffmpeg\ffmpeg.exe";
string rootVideos = @"D:\games\ffmpeg\test\";
var files = new List<string>()
{
    rootVideos + "1.mp4",
    rootVideos + "2.mp4",
    rootVideos + "3.mp4",
    rootVideos + "4.mp4",
    rootVideos + "5.mp4",
    rootVideos + "6.mp4",
};

string resultPath = string.Empty;

Log.Information("Запускаем");

var editor = new FFMpegEditor(ffmpeg).Configure(config =>
{
    files.ForEach(file => config.AddSrc(file));
    config.SaveTo("./", "result");
    resultPath = config.OutputFile.ToString();
});

Log.Debug($"Добавлены файлы из папки \"{rootVideos}\" ({files.Count})");

editor.CleanCache();

Log.Information("Кэшируем добавленные файлы");
await editor.CacheAsTsFormatAsync();

Log.Information("Обрабатываем...");
await editor.ConcatSourcesAsync();
Log.Information("Готово: " + File.Exists(resultPath));

Log.Information("Очищаем использованный кэш...");
editor.CleanCache();
Log.Information("Успешно");