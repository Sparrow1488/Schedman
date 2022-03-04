using Serilog;
using VideoSchedman;
using VideoSchedman.Entities;
using VideoSchedman.Enums;

Log.Logger = new LoggerConfiguration()
              .MinimumLevel
              .Debug()
              .WriteTo.Console()
              .CreateLogger();

string rootVideos = @"C:\Users\aleks\Downloads\videos";
var files = Directory.GetFiles(rootVideos).ToList();
string resultPath = string.Empty;

Log.Information("Запускаем");

Project.UseExistsProject("project_8f9fe638-bdd0-4acb-982c-976571b5809b");
var editor = new FFMpegEditor().Configure(config =>
                               //config.AddSrcRange(files)
                               config.RestoreSrc()
                               .SaveTo("Compilation")
                               .Quality(VideoQuality.FHD));

Log.Information($"Добавлены файлы из папки \"{rootVideos}\" ({files.Count})");

await editor.ConcatSourcesAsync(ConcatType.ReencodingConcatConvertedViaTransportStream);
Log.Information("Успешно");