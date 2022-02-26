using Serilog;
using VideoSchedman;
using VideoSchedman.Enums;

Log.Logger = new LoggerConfiguration()
              .MinimumLevel
              .Debug()
              .WriteTo.Console()
              .CreateLogger();

string rootVideos = @"D:\Закачки\ffmpeg_build\ffmpeg\bin\to_test";
var files = Directory.GetFiles(rootVideos).ToList();
string resultPath = string.Empty;

Log.Information("Запускаем");

var editor = new FFMpegEditor().Configure(config =>
                         config.AddSrcRange(files)
                               .SaveTo("Compilation")
                               .Quality(VideoQuality.FHD));
editor.OnCachedSource += (cached) => Log.Information(cached);
editor.OnConvertedSource += (converted) => Log.Information(converted);

Log.Information($"Добавлены файлы из папки \"{rootVideos}\" ({files.Count})");

await editor.ConcatSourcesAsync(ConcatType.ReencodingConcatConvertedViaTransportStream);
Log.Information("Успешно");