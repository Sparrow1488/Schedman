using Serilog;
using VideoSchedman;
using VideoSchedman.Enums;

Log.Logger = new LoggerConfiguration()
              .MinimumLevel
              .Debug()
              .WriteTo.Console()
              .CreateLogger();

string ffmpeg = @"D:\games\ffmpeg\ffmpeg-master-latest-win64-gpl-shared\bin\ffmpeg.exe";
string rootVideos = @"E:\Йога\Source-Films-Makers\отдельно sfm\52\3\cuts";
var files = Directory.GetFiles(rootVideos).ToList();
string resultPath = string.Empty;

Log.Information("Запускаем");

var editor = new FFMpegEditor(ffmpeg).Configure(config =>
{
    files.ForEach(file => config.AddSrc(file));
    config.SaveTo("./", "result")
          .Quality(VideoQuality.Preview);
    resultPath = config.OutputFile.ToString();
});

Log.Information($"Добавлены файлы из папки \"{rootVideos}\" ({files.Count})");

editor.CleanCache();

Log.Information("Кэшируем добавленные файлы");
await editor.CacheAsTsFormatAsync();

Log.Information("Обрабатываем...");
await editor.ConcatSourcesAsync();
Log.Information("Готово: " + File.Exists(resultPath));

Log.Information("Очищаем использованный кэш...");
//editor.CleanCache();
Log.Information("Успешно");