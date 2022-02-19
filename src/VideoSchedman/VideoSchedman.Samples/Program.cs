using Serilog;
using VideoSchedman;
using VideoSchedman.Enums;

Log.Logger = new LoggerConfiguration()
              .MinimumLevel
              .Debug()
              .WriteTo.Console()
              .CreateLogger();

string ffmpeg = @"D:\games\ffmpeg\ffmpeg.exe";
string rootVideos = @"C:\Users\aleks\OneDrive\Desktop\Илья\Repositories\VkSchedman\src\VideoSchedman\VideoSchedman.Samples\bin\Debug\net6.0\cached-files";
var files = Directory.GetFiles(rootVideos).ToList();
foreach (var file in files)
{
    var info = new FileInfo(file);
    File.Move(info.FullName, Path.Combine(info.DirectoryName, info.Name
                                        .Replace(";", "")
                                        .Replace("-", "")
                                        .Replace(" ", "")
                                        .Replace("'", "")
                                        .ToLower()));
}
files = Directory.GetFiles(rootVideos).ToList();

string resultPath = string.Empty;

Log.Information("Запускаем");

var editor = new FFMpegEditor(ffmpeg).Configure(config =>
{
    files.ForEach(file => config.AddSrc(file));
    config.SaveTo("./", "result").Quality(VideoQuality.Preview);
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